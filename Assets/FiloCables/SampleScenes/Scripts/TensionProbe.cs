using System;
using System.Collections.Generic;
using UnityEngine;
using Filo;

[RequireComponent(typeof(Cable))]
public class TensionProbe : MonoBehaviour
{
    Cable cable;
    public float tension = 0;
    public float cableTension = 0;

    public void Awake()
    {
        cable = GetComponent<Cable>();
    }

    public void Update(){
        tension = cable.sampledCable.Length / cable.RestLength;

        IList<CableJoint> joints = cable.Joints;
        foreach (CableJoint j in joints){
            float force = j.ImpulseMagnitude / Time.fixedDeltaTime;
            Debug.Log(force + " N, Mass:" + force/-9.81f +" Kg");
        }
    } 
}


