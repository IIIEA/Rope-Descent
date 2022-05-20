using UnityEngine;

public class CableComponent : MonoBehaviour
{
	#region Class members

	[SerializeField] private Transform _endPoint;
	[SerializeField] private Material _cableMaterial;

	[Header("Cable config")]
	[SerializeField] private float _cableLength = 0.5f;
	[SerializeField] private int _totalSegments = 5;
	[SerializeField] private float _segmentsPerUnit = 2f;
	[SerializeField] private float _cableWidth = 0.1f;

	[Header("Solver config")] 
	[SerializeField] private int _verletIterations = 1;
	[SerializeField] private int _solverIterations = 1;

	[Range(0,3)]
	[SerializeField] private float _stiffness = 1f;

	private int _segments = 0;
	private LineRenderer _line;
	private CableParticle[] _points;

	#endregion


	#region Initial setup

	private void Start()
	{
		InitCableParticles();
		InitLineRenderer();
	}

	private void InitCableParticles()
	{
		if (_totalSegments > 0)
			_segments = _totalSegments;
		else
			_segments = Mathf.CeilToInt(_cableLength * _segmentsPerUnit);

		Vector3 cableDirection = (_endPoint.position - transform.position).normalized;
		float initialSegmentLength = _cableLength / _segments;
		_points = new CableParticle[_segments + 1];

		for (int pointIdx = 0; pointIdx <= _segments; pointIdx++)
		{
			Vector3 initialPosition = transform.position + (cableDirection * (initialSegmentLength * pointIdx));
			_points[pointIdx] = new CableParticle(initialPosition);
		}

		CableParticle start = _points[0];
		CableParticle end = _points[_segments];
		start.Bind(this.transform);
		end.Bind(_endPoint.transform);
	}

	private void InitLineRenderer()
	{
		_line = gameObject.AddComponent<LineRenderer>();
		_line.SetWidth(_cableWidth, _cableWidth);
		_line.SetVertexCount(_segments + 1);
		_line.material = _cableMaterial;
		_line.GetComponent<Renderer>().enabled = true;
	}

	#endregion


	#region Render Pass

	private void Update()
	{
		RenderCable();
	}

	private void RenderCable()
	{
		for (int pointIdx = 0; pointIdx < _segments + 1; pointIdx++)
		{
			_line.SetPosition(pointIdx, _points[pointIdx].Position);
		}
	}

	#endregion


	#region Verlet integration & solver pass

	private void FixedUpdate()
	{
		for (int verletIdx = 0; verletIdx < _verletIterations; verletIdx++)
		{
			VerletIntegrate();
			SolveConstraints();
		}
	}

	private void VerletIntegrate()
	{
		Vector3 gravityDisplacement = Time.fixedDeltaTime * Time.fixedDeltaTime * Physics.gravity;

		foreach (CableParticle particle in _points)
		{
			particle.UpdateVerlet(gravityDisplacement);
		}
	}

	private void SolveConstraints()
	{
		for (int iterationIdx = 0; iterationIdx < _solverIterations; iterationIdx++)
		{
			SolveDistanceConstraint();
			SolveStiffnessConstraint();
		}
	}

	#endregion


	#region Solver Constraints

	private void SolveDistanceConstraint()
	{
		float segmentLength = _cableLength / _segments;

		for (int SegIdx = 0; SegIdx < _segments; SegIdx++)
		{
			CableParticle particleA = _points[SegIdx];
			CableParticle particleB = _points[SegIdx + 1];

			SolveDistanceConstraint(particleA, particleB, segmentLength);
		}
	}

	private void SolveDistanceConstraint(CableParticle particleA, CableParticle particleB, float segmentLength)
	{
		Vector3 delta = particleB.Position - particleA.Position;

		float currentDistance = delta.magnitude;
		float errorFactor = (currentDistance - segmentLength) / currentDistance;

		if (particleA.IsFree() && particleB.IsFree())
		{
			particleA.Position += errorFactor * 0.5f * delta;
			particleB.Position -= errorFactor * 0.5f * delta;
		}
		else if (particleA.IsFree())
		{
			particleA.Position += errorFactor * delta;
		}
		else if (particleB.IsFree())
		{
			particleB.Position -= errorFactor * delta;
		}
	}

	private void SolveStiffnessConstraint()
	{
		float distance = (_points[0].Position - _points[_segments].Position).magnitude;
		if (distance > _cableLength)
		{
			foreach (CableParticle particle in _points)
			{
				SolveStiffnessConstraint(particle, distance);
			}
		}
	}

	private void SolveStiffnessConstraint(CableParticle cableParticle, float distance)
	{


	}

	#endregion
}
