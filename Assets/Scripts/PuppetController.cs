using UnityEngine;
using RootMotion.Dynamics;

public class PuppetController : MonoBehaviour, IDamageable
{
    private PuppetMaster _puppet;

    private void Start()
    {
        _puppet = GetComponentInChildren<PuppetMaster>();
    }

    public void TakeDamage()
    {
        _puppet.mode = PuppetMaster.Mode.Active;
        _puppet.state = PuppetMaster.State.Dead;
    }
}
