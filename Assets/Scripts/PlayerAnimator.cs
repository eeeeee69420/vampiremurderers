using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public PlayerController playerController;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerController>();
    }
    void Update()
    {
        if (playerController.inputDirection.magnitude > 0.2)
            animator.SetBool("isMoving", true);
        else
            animator.SetBool("isMoving", false);
    }
    public void PlayAnimation(string Animation)
    {
        animator.Play(Animation);
    }
}
