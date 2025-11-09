using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterController characterController;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        characterController = GetComponent<PlayerController>();
    }
    public void PlayAnimation(string Animation)
    {
        animator.Play(Animation);
    }
}
