using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    public Animator animator;
    public EnemyBase enemyController;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        enemyController = GetComponent<EnemyBase>();
    }
    private void Update()
    {
        if (enemyController.direction == Vector2.zero)
            animator.SetBool("IsIdling", true);
        else
            animator.SetBool("IsIdling", false);
    }
    public void PlayAnimation(string Animation)
    {
        animator.Play(Animation, 0);
    }
}
