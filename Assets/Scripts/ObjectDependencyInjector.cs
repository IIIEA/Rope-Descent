using UnityEngine;

public class ObjectDependencyInjector : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;

    private IDependency<GameObject>[] _dependencies;

    public GameObject GameObject
    {
        get => _gameObject;
        set
        {
            if (_gameObject == value)
                return;

            _gameObject = value;
            Inject();
        }
    }

    private void Start()
    {
        _dependencies = GetComponents<IDependency<GameObject>>();

        if (_gameObject != null)
            Inject();
    }

    private void Inject()
    {
        foreach (var dependency in _dependencies)
        {
            dependency.Inject(GameObject);
        }
    }
}
