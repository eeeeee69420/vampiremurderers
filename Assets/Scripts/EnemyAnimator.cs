using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    public Animator animator;
    public EnemyMovement enemyMovement;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        enemyMovement = GetComponent<EnemyMovement>();
    }
    // Update is called once per frame
    public void PlayAnimation(string Animation)
    {
        animator.Play(Animation);
    }
}
