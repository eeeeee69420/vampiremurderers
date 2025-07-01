using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    public float attackCooldown;
    public float remainingCooldown;
    public float attackAnimationDuration;
    public EnemyController enemyController;
    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (remainingCooldown > 0)
            remainingCooldown -= Time.fixedDeltaTime;
        else if (remainingCooldown <= 0)
        {
            remainingCooldown = 0;
            if (enemyController.touchingPlayer)
                StartCoroutine(AttackPlayer());
        }
    }
    IEnumerator AttackPlayer()
    {
        enemyController.enemyAnimator.PlayAnimation("Attack");
        remainingCooldown = attackCooldown;
        yield return new WaitForSeconds(attackAnimationDuration);
        if (enemyController.touchingPlayer)
        {
            enemyController.controller.hitScreenAnim();
        }
    }
}
