using System;
using UnityEngine;

public class DistanceChecker : MonoBehaviour
{
    [SerializeField] private Transform _point;
    [SerializeField] private float _plusDistnceSize;
    [SerializeField] private float _maxDistance;

    private float _distancePassed = 0;
    private float _currentDistance;

    public float StartDistance { get; private set; }

    public float MaxDistance 
    {
        get
        {
            return _maxDistance;
        } 
        private set
        {
            _maxDistance = value;
        }
    }

    public event Action<float, float> DistanceChanged;

    private void Start()
    {
        StartDistance = (transform.position - _point.position).magnitude;
        MaxDistance = _maxDistance + StartDistance;
        DistanceChanged?.Invoke(StartDistance, MaxDistance);
        _currentDistance = StartDistance;
    }

    private void Update()
    {
        if(_currentDistance != (transform.position - _point.position).magnitude)
        {
            _currentDistance = (transform.position - _point.position).magnitude - _distancePassed;

            if(_currentDistance < 0)
            {
                _currentDistance = 0;
            }

            DistanceChanged?.Invoke(_currentDistance, MaxDistance);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<RopeBuff>())
        {
            _distancePassed += _plusDistnceSize;
        }
    }
}
