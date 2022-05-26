using System.Collections;
using UnityEngine;
using DG.Tweening;

public class RestartButtonContainer : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private float _offset;

    private Sequence _sequence;
    private Vector3 _startPosition;
    private Camera _camera;

    private void Awake()
    {
        _startPosition = transform.position;
        _camera = Camera.main;
        transform.position = new Vector3(transform.position.x, _camera.WorldToViewportPoint(transform.position).y - _offset, transform.position.z);
        gameObject.SetActive(false);
    }
    
    public void ShowUp()
    {
        gameObject.SetActive(true);
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        yield return new WaitForSeconds(_delay);

        _sequence = DOTween.Sequence();

        _sequence.Append(transform.DOMoveY(_startPosition.y + _offset, 0.3f));
        _sequence.Append(transform.DOMoveY(_startPosition.y, 0.3f));
    }
}
