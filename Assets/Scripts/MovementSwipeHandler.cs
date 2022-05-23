using UnityEngine;

public class MovementSwipeHandler : MonoBehaviour, IMovableObjectHandler
{
    [SerializeField] private Transform _leftBorder;
    [SerializeField] private Transform _rightBorder;
    [Header("Slowdown coefficient on swipe"), Range(0.5f, 3f)]
    [SerializeField] private float _normalizedCoefficient = 1.0f;
    [SerializeField] private float _speed;

    private float _targetPosX;
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

    private void FixedUpdate()
    {
        _movableObject.transform.position = Vector3.MoveTowards(_movableObject.transform.position, new Vector3(_targetPosX, _movableObject.transform.position.y, _movableObject.transform.position.z), _speed * Time.deltaTime);
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

        _targetPosX = currentPos.x + offset;
        CheckPosition(_movableObject.transform.position);
    }

    private void CheckPosition(Vector3 currentPositionX)
    {
        if(currentPositionX.x <= _rightBorder.position.x && currentPositionX.x >= _leftBorder.position.x)
        {
            return;
        }

        var newPositionX = currentPositionX.x;

        if (currentPositionX.x > _rightBorder.position.x)
            newPositionX = _rightBorder.position.x;
        else if (currentPositionX.x < _leftBorder.position.x)
            newPositionX = _leftBorder.position.x;

        currentPositionX = PositionSetter.SetPositionX(currentPositionX, newPositionX);
        _movableObject.transform.position = currentPositionX;
    }
}