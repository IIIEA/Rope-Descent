using System;
using UnityEngine;
using UnityEngine.Events;

public class Jumper : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private SwipeMouseDetector _swipeMouseDetector;
    [Header("Parameters")]
    [SerializeField] private float _wallContactPosition;
    [SerializeField] private float _maxJumpHeight;
    [SerializeField] private float _velocity;

    private float _jumpHeight;
    private bool _isGrounded = true;
    private JumpStates _currentJumpState;

    public bool IsGrounded => _isGrounded;

    public UnityAction Landed;
    public UnityAction Jumped;

    private void Start()
    {
        _currentJumpState = JumpStates.Fall;
        _jumpHeight = _wallContactPosition - _maxJumpHeight;
    }

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
        switch (_currentJumpState)
        {
            case JumpStates.StartJump:
                Jump();
                break;
            case JumpStates.Fall:
                Fall();
                break;
        }
    }

    private void OnSwipe(Vector2 delta)
    {
        SetJumpState(JumpStates.StartJump);
    }

    private void OnSwipeEnd(Vector2 delta)
    {
        _isGrounded = false;
        SetJumpState(JumpStates.Fall);
    }

    private void Jump()
    {
        Jumped?.Invoke();
        _isGrounded = false;

        if (transform.position.z <= _jumpHeight)
        {
            transform.position = PositionSetter.SetPositionZ(transform.position, _jumpHeight);
            return;
        }

        Vector3 pos = transform.position;

        pos.z = Mathf.MoveTowards(pos.z, _jumpHeight, _velocity * Time.deltaTime);

        transform.position = PositionSetter.SetPositionZ(transform.position, pos.z);

    }

    private void Fall()
    {
        if (_isGrounded == false)
        {
            if (transform.position.z >= _wallContactPosition)
            {
                _isGrounded = true;
                Landed?.Invoke();
                transform.position = PositionSetter.SetPositionZ(transform.position, _wallContactPosition);
                return;
            }

            Vector3 pos = transform.position;

            pos.z = Mathf.MoveTowards(pos.z, _wallContactPosition, _velocity * Time.deltaTime);

            transform.position = PositionSetter.SetPositionZ(transform.position, pos.z);
        }
    }

    private void SetJumpState(JumpStates newState)
    {
        if (_currentJumpState == newState)
            return;

        _currentJumpState = newState;
    }

    private enum JumpStates
    {
        StartJump,
        Fall
    }
}