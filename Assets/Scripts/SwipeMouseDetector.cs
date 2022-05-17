using System;
using UnityEngine;

public class SwipeMouseDetector : MonoBehaviour, ISwipe
{
    private bool _isSwipe;
    private Vector3 _lastPosition = new Vector2();

    public event Action<Vector2> Swipe;
    public event Action<Vector2> SwipeEnd;

    private void Start()
    {
        _lastPosition = Input.mousePosition;
    }

    private void FixedUpdate()
    {
        if (!Input.GetMouseButton(0))
        {
            if (_isSwipe)
            {
                _isSwipe = false;
                SwipeEnd?.Invoke(_lastPosition);
            }

            _lastPosition = Input.mousePosition;
            return;
        }

        if (_isSwipe == false)
        {
            _isSwipe = true;
        }

        Swipe?.Invoke(Input.mousePosition - _lastPosition);
        _lastPosition = Input.mousePosition;
    }
}

