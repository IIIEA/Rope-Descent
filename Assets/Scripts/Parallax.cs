using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _speed;

    private float _offset;

    private void Start()
    {
        _offset = transform.position.y - _camera.transform.position.y;   
    }

    private void FixedUpdate()
    {
        var targetPositionY = new Vector3(transform.position.x, _offset + _camera.transform.position.y, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, targetPositionY, _speed * Time.deltaTime);
    }
}
