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

            _swipeDetector.Swipe += OnSwipe;
        }
        else
        {
            Debug.LogError("ISwipe component in null");
        }
    }

    private void OnDestroy()
    {
        _swipeDetector.Swipe -= OnSwipe;
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

        _movableObject.transform.position = PositionSetter.SetPositionX(_movableObject.transform.position, currentPos.x + offset);
        _movableObject.transform.position = SetPosition(_movableObject.transform.position);
    }

    private Vector3 SetPosition(Vector3 currentPositionX)
    {
        var newPositionX = currentPositionX.x;

        if (currentPositionX.x > _rightBorder.position.x)
            newPositionX = _rightBorder.position.x;
        else if (_movableObject.transform.position.x < _leftBorder.position.x)
            newPositionX = _leftBorder.position.x;

        currentPositionX = PositionSetter.SetPositionX(currentPositionX, newPositionX);

        return currentPositionX;
    }
}