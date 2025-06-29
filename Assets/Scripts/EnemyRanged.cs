using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    public float attackCooldown;
    public float remainingCooldown;
    public float range;
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
            if (enemyMovement.targetDistance <= range)
                StartCoroutine(ThrowProjectile());
        }
    }
    IEnumerator ThrowProjectile()
    {
        enemyMovement.enemyAnimator.PlayAnimation("RangedAttack");
        remainingCooldown = attackCooldown;
        yield return null;
    }
}
