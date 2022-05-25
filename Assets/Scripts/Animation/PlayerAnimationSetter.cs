using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationSetter : MonoBehaviour
{
    [SerializeField] private Jumper _jumper;

    private Animator _animator;

    private const string Land = nameof(Land);
    private const string Grounded = nameof(Grounded);

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetBool(Grounded, _jumper.IsGrounded);
    }

    private void OnEnable()
    {
        _jumper.Landed += OnLanded;
    }

    private void OnDisable()
    {
        _jumper.Landed -= OnLanded;
    }

    private void OnLanded()
    {
        _animator.SetTrigger(Land);
    }
}
