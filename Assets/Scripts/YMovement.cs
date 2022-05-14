using System;
using UnityEngine;

public class YMovementHandler : MonoBehaviour, IMovableObjectHandler
{
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
    }
}