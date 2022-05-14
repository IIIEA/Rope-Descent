using System;
using UnityEngine;

public class JumpZ : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private SwipeMouseDetector _swipeMouseDetector;
    [Header("Parameters")]
    [SerializeField] private float _wallContactPosition;
    [SerializeField] private float _maxJumpHeight;
    [SerializeField] private float _velocity;

    private bool _isGrounded;

    private void OnEnable()
    {
        _swipeMouseDetector.Swipe += OnSwipe;
    }

    private void OnDestroy()
    {
        _swipeMouseDetector.Swipe -= OnSwipe;
    }

    private void FixedUpdate()
    {
        Jump();

        if(transform.position.z <= _wallContactPosition)
        {
            _isGrounded = true;
        }
    }

    private void Jump()
    {
        Vector3 pos = transform.position;

        if (_isGrounded == false)
        {
            pos.z += _velocity * Time.deltaTime;

            if(pos.z >= _wallContactPosition + _maxJumpHeight)
            {
                pos.z = _wallContactPosition + _maxJumpHeight;
            }

            transform.position = PositionSetter.SetPositionZ(transform.position, pos.z);
        }
    }

    private void Fall()
    {

    }

    private void OnSwipe(Vector2 delta)
    {
        _isGrounded = false;
    }
}