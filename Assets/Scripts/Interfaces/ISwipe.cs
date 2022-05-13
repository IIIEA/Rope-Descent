using System;
using UnityEngine;

public interface ISwipe
{
    public event Action<Vector2> OnSwipe;
    public event Action<Vector2> OnSwipeEnd;
}
