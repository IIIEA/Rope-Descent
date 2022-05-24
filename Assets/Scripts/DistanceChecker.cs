using System;
using UnityEngine;

public class DistanceChecker : MonoBehaviour
{
    [SerializeField] private Transform _startPosition;
    [SerializeField] private float _plusDistnceSize;
    [SerializeField] private float _maxDistance;

    private float _distancePassed = 0;
    private float _points = 0;
    private float _currentDistance;

    public float MaxDistance => _maxDistance;

    public event Action<float, float> DistanceChanged;

    private void Start()
    {
        _startPosition.position = transform.position;
        DistanceChanged?.Invoke(_points, MaxDistance);
        _currentDistance = Mathf.Abs(transform.position.y - _startPosition.position.y);
    }

    private void Update()
    {
        if(_currentDistance != Mathf.Abs(transform.position.y - _startPosition.position.y))
        {
            _currentDistance = Mathf.Abs(transform.position.y - _startPosition.position.y);
            var points = _currentDistance - _distancePassed;
            _distancePassed += points;
            _points += points;

            DistanceChanged?.Invoke(_points, MaxDistance);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ICollectable>(out ICollectable collectable))
        {
            _points -= _plusDistnceSize;

            if(_points < 0)
            {
                _points = 0;
            }

            collectable.Collect(transform.position);
        }
    }
}
