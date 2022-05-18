using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RopeBar : MonoBehaviour
{
    [SerializeField] private DistanceChecker _distanceChecker;
    [SerializeField] private float _duration;

    private Slider _slider;

    private void Start()
    {
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
}
