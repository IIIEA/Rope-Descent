using System;
using UnityEngine;

public class YMovement : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private SwipeMouseDetector _swipeMouseDetector;
    [Header("Parameters")]
    [SerializeField] private float _currentVelocity;
    [SerializeField] private float _maxVelocity;
    [SerializeField] private float _minVelocity;


    private Vector3 _velocity;

    private void OnEnable()
    {
        _swipeMouseDetector.Swipe += OnSwipe;
    }

    private void OnDisable()
    {
        _swipeMouseDetector.Swipe -= OnSwipe;
    }

    private void OnSwipe(Vector2 delta)
    {
        Vector3 pos = transform.position;

        pos.y += _velocity.y * Time.deltaTime;

        _velocity.y = Mathf.Lerp(_velocity.y, _maxVelocity, Time.deltaTime);

        Debug.Log(_velocity.y);

        transform.position = PositionSetter.SetPositionY(transform.position, pos.y);
    }
}