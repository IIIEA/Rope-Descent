using UnityEngine;
using RootMotion.Dynamics;
using UnityEngine.Events;

public class PuppetController : MonoBehaviour, IDamageable
{
    private PuppetMaster _puppet;

    public UnityAction Died;

    private void Start()
    {
        _puppet = GetComponentInChildren<PuppetMaster>();
    }

    public void TakeDamage()
    {
        Died?.Invoke();
        _puppet.mode = PuppetMaster.Mode.Active;
        _puppet.state = PuppetMaster.State.Dead;
    }
}
