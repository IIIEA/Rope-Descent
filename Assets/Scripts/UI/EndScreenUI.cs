using UnityEngine;
using UnityEngine.Events;

public class EndScreenUI : MonoBehaviour
{
    [SerializeField] private PuppetController _puppetController;

    public UnityEvent GameOver;

    private void OnEnable()
    {
        _puppetController.Died += OnDied;
    }

    private void OnDisable()
    {
        _puppetController.Died -= OnDied;
    }

    private void OnDied()
    {
        GameOver?.Invoke();
    }
}
