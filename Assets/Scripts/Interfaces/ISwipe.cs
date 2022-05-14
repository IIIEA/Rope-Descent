using System;
using UnityEngine;

public interface ISwipe
{
    public event Action<Vector2> Swipe;
    public event Action<Vector2> SwipeEnd;
}
