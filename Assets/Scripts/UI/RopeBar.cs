using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class RopeBar : MonoBehaviour
{
    [SerializeField] private DistanceChecker _distanceChecker;
    [SerializeField] private float _duration;
    [SerializeField] private float _offset;

    private RectTransform _rect;
    private Sequence _sequence;
    private Slider _slider;

    private void Start()
    {
        _rect = GetComponent<RectTransform>();
        _slider = GetComponentInChildren<Slider>();
        _slider.value = _slider.maxValue;
    }

    private void OnEnable()
    {
        _distanceChecker.DistanceChanged += OnValueChanged;
    }

    private void OnDisable()
    {
        _distanceChecker.DistanceChanged -= OnValueChanged;
    }

    private void OnValueChanged(float value, float maxValue)
    {
        if (maxValue != 0)
        {
            _slider.DOValue(_slider.maxValue - (value / maxValue), _duration);
        }
    }

    public void OnEndGame()
    {
        Vector3 point = Camera.main.WorldToViewportPoint(transform.position);

        _sequence = DOTween.Sequence();
        _sequence.Append(transform.DOMoveX(_rect.position.x + _offset, 0.2f));
        _sequence.Append(transform.DOMoveX(point.x - _offset, 0.2f));
    }
}
