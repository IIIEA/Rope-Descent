using System;
using UnityEngine;

public class DistanceChecker : MonoBehaviour
{
    [SerializeField] private Transform _point;

    private float _lastDistance;

    public event Action<float> DistanceChanged;

    private void Start()
    {
        _lastDistance = (transform.position - _point.position).magnitude;
        DistanceChanged?.Invoke(_lastDistance);
    }

    private void Update()
    {
        if(_lastDistance != (transform.position - _point.position).magnitude)
        {
            _lastDistance = (transform.position - _point.position).magnitude;
            DistanceChanged?.Invoke(_lastDistance);
        }
    }
}
