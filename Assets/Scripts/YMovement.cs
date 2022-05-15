using System;
using UnityEngine;

public class YMovementHandler : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private SwipeMouseDetector _swipeMouseDetector;
    [Header("Parameters")]
    [SerializeField] private float _currentVelocity;
    [SerializeField] private float _maxVelocity;
    [SerializeField] private float _minVelocity;

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

    }
}