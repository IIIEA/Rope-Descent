using System;
using UnityEngine;

public class XMovementSwipeHandler : MonoBehaviour, IMovableObjectHandler
{
    [SerializeField] private Transform _leftBorder;
    [SerializeField] private Transform _rightBorder;
    [Header("Slowdown coefficient on swipe"), Range(0.5f, 1.5f)]
    [SerializeField] private float _normalizedCoefficient = 1.0f;

    private GameObject _movableObject;
    private ISwipe _swipeDetector;

    private void Start()
    {
        if (TryGetComponent<ISwipe>(out ISwipe iSwipe))
        {
            _swipeDetector = iSwipe;

            _swipeDetector.OnSwipe += OnSwipe;
        }
        else
        {
            Debug.LogError("ISwipe component in null");
        }
    }

    private void OnDestroy()
    {
        _swipeDetector.OnSwipe -= OnSwipe;
    }

    public void Inject(GameObject dependency)
    {
        _movableObject = dependency;
    }

    private void OnSwipe(Vector2 delta)
    {
        if (_movableObject == null)
        {
            return;
        }

        if (Mathf.Abs(delta.x - Mathf.Epsilon) <= 0)
            return;

        var borderDistance = Mathf.Abs(_rightBorder.position.x - _leftBorder.position.x);
        var offset = borderDistance * _normalizedCoefficient * delta.x / Screen.width;
        var currentPos = _movableObject.transform.position;

        _movableObject.transform.position = new Vector3(currentPos.x + offset, currentPos.y, currentPos.z);

        _movableObject.transform.position = SetPosition(_movableObject.transform.position);
    }

    private Vector3 SetPosition(Vector3 currentPositionX)
    {
        if (currentPositionX.x > _rightBorder.position.x)
            currentPositionX = new Vector3(_rightBorder.transform.position.x, currentPositionX.y, currentPositionX.z);
        else if (_movableObject.transform.position.x < _leftBorder.position.x)
            currentPositionX = new Vector3(_leftBorder.transform.position.x, currentPositionX.y, currentPositionX.z);

        return currentPositionX;
    }
}