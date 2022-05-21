using UnityEngine;

public class SelfRotator : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;

    void Update()
    {
        transform.Rotate(0, 0, _rotateSpeed * Time.deltaTime);
    }
}
