using UnityEngine;
using Unity.Mathematics;

namespace RopeMinikit
{
    public enum RopeConnectionType : int
    {
        //Pin a point on the rope to a point on the transform. The transform can move freely and the rope will always follow.
        PinRopeToTransform = 0,
        //Pin a point on the transform to a point on the rope. The rope will move freely and the transform will always follow. This connection controls the transition.
        PinTransformToRope = 1,
        //Pulls a point on the rigid body toward a point on the rope by applying a velocity change to the rigid body. This connection does not control the rigid body, other forces and constraints are respected.
        PullRigidbodyToRope = 2,
        //Introduces a two-way coupling between the rope and the rigid body. The rope will react to the rigid body and feed pulses back to the rigid body, allowing complex setups such as the crane in the example scene.
        //Care must be taken to make the rope mass per meter value comparable to the mass of the connected rigid body, otherwise the simulation may explode. This connection does not control the rigid body, other forces and constraints are respected.
        TwoWayCouplingBetweenRigidbodyAndRope = 3,
    }

    [RequireComponent(typeof(Rope))]
    public class RopeConnection : MonoBehaviour
    {
        protected static readonly Color[] colors = new Color[4]
        {
            new Color(0.69f, 0.0f, 1.0f), // purple
            new Color(1.0f, 0.0f, 0.0f), // red
            new Color(1.0f, 0.0f, 0.0f), // red
            new Color(1.0f, 1.0f, 0.0f), // yellow
        };

        [System.Serializable]
        public struct RigidbodySettings
        {
            [Tooltip("The rigidbody to connect to")]
            public Rigidbody body;

            [Tooltip("A measure of connection stiffness. Lower values are generally more stable.")]
            [Range(0.0f, 1.0f)] public float stiffness;

            [Tooltip("The amount of rigid body velocity to remove when a pulse from the rope is applied to the rigid body")]
            [Range(0.0f, 1.0f)] public float damping;
        }

        [System.Serializable]
        public struct TransformSettings
        {
            [Tooltip("The transform to connect to")]
            public Transform transform;
        }

        [DisableInPlayMode] public RopeConnectionType type;
        [DisableInPlayMode, Range(0.0f, 1.0f)] public float ropeLocation;
        public bool autoFindRopeLocation = false;

        public RigidbodySettings rigidbodySettings = new RigidbodySettings()
        {
            stiffness = 0.15f,
            damping = 0.05f,
        };

        public TransformSettings transformSettings = new TransformSettings()
        {};

        [Tooltip("the point to connect in local object space")]
        public float3 localConnectionPoint;

        protected Rope rope;
        protected int particleIndex;

        public Component connectedObject
        {
            get
            {
                switch (type)
                {
                    case RopeConnectionType.PinRopeToTransform:
                    case RopeConnectionType.PinTransformToRope: {
                        return transformSettings.transform;
                    }
                    case RopeConnectionType.PullRigidbodyToRope:
                    case RopeConnectionType.TwoWayCouplingBetweenRigidbodyAndRope: {
                        return rigidbodySettings.body;
                    }
                    default: {
                        return null;
                    }
                }
            }
        }

        public float3 connectionPoint
        {
            get
            {
                var obj = connectedObject;
                if (obj)
                {
                    return obj.transform.TransformPoint(localConnectionPoint);
                }
                else
                {
                    return float3.zero;
                }
            }
        }

        public void Init(bool forceReset)
        {
            if (rope && !forceReset)
            {
                return;
            }

            rope = GetComponent<Rope>();
            Debug.Assert(rope); // required component!

            if (autoFindRopeLocation)
            {
                rope.GetClosestParticle(connectionPoint, out particleIndex, out float distance);
                ropeLocation = rope.GetScalarDistanceAt(particleIndex);
            }
            else
            {
                var ropeDistance = ropeLocation * rope.measurements.realCurveLength;
                particleIndex = rope.GetParticleIndexAt(ropeDistance);
            }
        }

        public void OnRopeSplit(Rope.OnSplitParams p)
        {
            if (autoFindRopeLocation)
            {
                // There is no way to determine which side of the split this component was located, just remove it...
                Destroy(this);
            }
            else
            {
                var idx = p.preSplitMeasurements.GetParticleIndexAt(ropeLocation * p.preSplitMeasurements.realCurveLength);
                if (idx < p.minParticleIndex || idx > p.maxParticleIndex)
                {
                    Destroy(this);
                }
            }
        }

        public void OnDisable()
        {
            if (rope && type == RopeConnectionType.PinRopeToTransform)
            {
                rope.SetMassMultiplierAt(particleIndex, 1.0f);
            }
        }

        public void FixedUpdate()
        {
            Init(false);

            if (!rope || !connectedObject)
            {
                return;
            }

            switch (type)
            {
                case RopeConnectionType.PinRopeToTransform:
                {
                    rope.SetMassMultiplierAt(particleIndex, 0.0f);
                    rope.SetPositionAt(particleIndex, connectionPoint);
                    break;
                }
                case RopeConnectionType.PinTransformToRope:
                {
                    var target = rope.GetPositionAt(particleIndex);
                    var offset = (float3)(transformSettings.transform.TransformPoint(localConnectionPoint) - transformSettings.transform.position);
                    transformSettings.transform.position = target - offset;
                    break;
                }
                case RopeConnectionType.PullRigidbodyToRope:
                {
                    var target = rope.GetPositionAt(particleIndex);
                    var current = connectionPoint;
                    var delta = target - current;
                    var dist = math.length(delta);
                    if (dist > 0.0f)
                    {
                        var normal = delta / dist;
                        var correctionVelocity = dist * rigidbodySettings.stiffness / Time.fixedDeltaTime;
                        rigidbodySettings.body.SetPointVelocityNow(current, normal, correctionVelocity, rigidbodySettings.damping);
                    }
                    break;
                }
                case RopeConnectionType.TwoWayCouplingBetweenRigidbodyAndRope:
                {
                    rope.RegisterRigidbodyConnection(
                        particleIndex,
                        rigidbodySettings.body,
                        rigidbodySettings.damping,
                        connectionPoint,
                        rigidbodySettings.stiffness);
                    break;
                }
            }
        }

#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                return;
            }
            var rope = GetComponent<Rope>();
            if (!rope || rope.spawnPoints.Count < 2 || !connectedObject)
            {
                return;
            }

            var objPoint = connectionPoint;

            Gizmos.color = colors[(int)type];

            Gizmos.DrawWireCube(objPoint, Vector3.one * 0.05f);

            if (!autoFindRopeLocation)
            {
                var localToWorld = (float4x4)rope.transform.localToWorldMatrix;
                var ropeLength = rope.spawnPoints.GetLengthOfCurve(ref localToWorld);
                rope.spawnPoints.GetPointAlongCurve(ref localToWorld, ropeLength * ropeLocation, out float3 ropePoint);

                Gizmos.DrawWireCube(ropePoint, Vector3.one * 0.05f);
                Gizmos.DrawLine(ropePoint, objPoint);
            }
        }
#endif
    }
}
