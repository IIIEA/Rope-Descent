using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationSetter : MonoBehaviour
{
    [SerializeField] private Jumper _jumper;
    [SerializeField] private YMovement _yMovement;
    [SerializeField] private ParticleSystem _jumpParticles;

    private Animator _animator;

    private const string Land = nameof(Land);
    private const string SlideDown = nameof(SlideDown);
    private const string VelocityY = nameof(VelocityY);

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(_jumper.IsGrounded == false)
        {
            _animator.Play(SlideDown);
        }

        _animator.SetFloat(VelocityY, Remap.DoRemap(0, 10, 0, 1, _yMovement.VelocityY));
    }

    private void OnEnable()
    {
        _jumper.Landed += OnLanded;
        _jumper.Jumped += OnJumped;
    }

    private void OnDisable()
    {
        _jumper.Landed -= OnLanded;
        _jumper.Jumped -= OnJumped;
    }

    private void OnLanded()
    {
        _animator.SetTrigger(Land);
    }

    private void OnJumped()
    {
        if (_jumper.IsGrounded)
        {
            _jumpParticles.Play();
        }
    }
}
