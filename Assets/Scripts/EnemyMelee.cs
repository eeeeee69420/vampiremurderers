using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    public float attackCooldown;
    public float remainingCooldown;
    public float attackAnimationDuration;
    public EnemyMovement enemyMovement;
    // Start is called before the first frame update
    void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (remainingCooldown > 0)
            remainingCooldown -= Time.fixedDeltaTime;
        else if (remainingCooldown <= 0)
        {
            remainingCooldown = 0;
            if (enemyMovement.touchingPlayer)
                StartCoroutine(AttackPlayer());
        }
    }
    IEnumerator AttackPlayer()
    {
        enemyMovement.enemyAnimator.PlayAnimation("Attack");
        remainingCooldown = attackCooldown;
        yield return new WaitForSeconds(attackAnimationDuration);
        if (enemyMovement.touchingPlayer)
        {
            enemyMovement.controller.hitScreenAnim();
        }
    }
}
