using UnityEngine;
using RootMotion.Dynamics;
using UnityEngine.Events;

public class PuppetController : MonoBehaviour, IDamageable
{
    private YMovement _yMovement;
    private Jumper _jumper;
    private PuppetController _controller;
    private PuppetMaster _puppet;

    public UnityAction Died;

    private void Start()
    {
        _jumper = GetComponent<Jumper>();
        _yMovement = GetComponent<YMovement>();
        _controller = GetComponent<PuppetController>();
        _puppet = GetComponentInChildren<PuppetMaster>();
    }

    public void TakeDamage()
    {
        Died?.Invoke();
        _puppet.mode = PuppetMaster.Mode.Active;
        _puppet.state = PuppetMaster.State.Dead;
        _jumper.enabled = false;
        _yMovement.enabled = false;
        _controller.enabled = false;
    }
}
