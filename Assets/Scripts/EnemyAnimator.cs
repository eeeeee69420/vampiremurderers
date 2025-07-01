using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    public Animator animator;
    public EnemyController enemyController;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        enemyController = GetComponent<EnemyController>();
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
        animator.Play(Animation);
    }
}
