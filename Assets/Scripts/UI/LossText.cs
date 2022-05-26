using UnityEngine;
using DG.Tweening;

public class LossText : MonoBehaviour
{
    [SerializeField] private float _scale;

    private Vector3 _startScale;
    private Sequence _sequence;

    private void Awake()
    {
        _startScale = transform.localScale;
        gameObject.SetActive(false);
        transform.localScale = Vector3.zero;
    }

    public void ShowUp()
    {
        _sequence = DOTween.Sequence();
        gameObject.SetActive(true);

        _sequence.Append(transform.DOScale(_startScale * _scale, 0.3f));
        _sequence.Append(transform.DOScale(_startScale, 0.3f));
    }
}
