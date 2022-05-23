using UnityEngine;

public class SelfRotator : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private Vector3 _axis;

    private Vector3 _targetAxis;

    private void Start()
    {
        _targetAxis = _axis.normalized * _rotateSpeed * Time.deltaTime;    
    }

    void Update()
    {
        transform.Rotate(_targetAxis.x, _targetAxis.y, _targetAxis.z);
    }
}
