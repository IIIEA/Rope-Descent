using System;
using UnityEngine;

public class DistanceChecker : MonoBehaviour
{
    [SerializeField] private Transform _point;
    [SerializeField] private float _plusDistnceSize;
    [SerializeField] private float _maxDistance;

    private float _distancePassed = 0;
    private float _points = 0;
    private float _currentDistance;

    public float MaxDistance => _maxDistance;

    public event Action<float, float> DistanceChanged;

    private void Start()
    {
        DistanceChanged?.Invoke(_distancePassed, MaxDistance);
        _currentDistance = (transform.position - _point.position).magnitude;
    }

    private void Update()
    {
        if(_currentDistance != (transform.position - _point.position).magnitude)
        {
            _currentDistance = (transform.position - _point.position).magnitude;
            var points = _currentDistance - _distancePassed;
            _points += points;
            _distancePassed += points;

            Debug.Log(_points);
            DistanceChanged?.Invoke(_points, MaxDistance);


        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<RopeBuff>())
        {
            _points -= _plusDistnceSize;
        }
    }
}
