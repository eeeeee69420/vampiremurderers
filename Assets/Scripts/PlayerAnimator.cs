using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;
    public PlayerController playerController;
    // Start is called before the first frame update
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
    // Update is called once per frame
    public void PlayAnimation(string Animation)
    {
        animator.Play(Animation);
    }
}
