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
    [SerializeField] private float _speed;


    private Vector3 _velocity;
    private bool _isSlide;

    public float VelocityY => _velocity.y;

    private void OnEnable()
    {
        _swipeMouseDetector.Swipe += OnSwipe;
        _swipeMouseDetector.SwipeEnd += OnSwipeEnd;
    }

    private void OnDisable()
    {
        _swipeMouseDetector.Swipe -= OnSwipe;
        _swipeMouseDetector.SwipeEnd -= OnSwipeEnd;
    }

    private void FixedUpdate()
    {
        SlideDown();
    }

    private void OnSwipe(Vector2 delta)
    {
        _isSlide = true;
    }

    private void OnSwipeEnd(Vector2 delta)
    {
        _isSlide = false;
    }

    private void SlideDown()
    {
        Vector3 pos = transform.position;

        pos.y -= _velocity.y * Time.deltaTime;

        if (_isSlide)
        {
            _velocity.y = Mathf.Lerp(_velocity.y, _maxVelocity, Time.deltaTime);
        }
        else
        {
            if (_velocity.y >= _minVelocity)
            {
                _velocity.y = Mathf.Lerp(_velocity.y, 0, _speed * Time.deltaTime);
            }
            else
            {
                _velocity.y = 0;
            }
        }

        transform.position = PositionSetter.SetPositionY(transform.position, pos.y);
    }
}