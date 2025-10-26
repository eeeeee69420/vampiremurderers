using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedHold : EnemyBase
{
    public float range;
    public float preferredDistance;
    public float preferredDistanceRange;
    public GameObject projectile;


    protected override void TryAttack()
    {
        if (closestDistance <= range && remainingCooldown <= 0)
            StartCoroutine(AttackPlayer());
    }
    protected override void Move()
    {
        if (closestDistance < preferredDistance - preferredDistanceRange)
            direction *= -1f;
        else if (closestDistance > preferredDistance + preferredDistanceRange) { }
        else
            direction = Vector2.zero;
        enemyBody.MovePosition(enemyBody.position + speed * Time.fixedDeltaTime * direction);
    }
    protected override IEnumerator AttackPlayer()
    {
        enemyAnimator.animator.SetTrigger("IsThrowing");
        remainingCooldown += attackCooldown;
        yield return new WaitForSeconds(attackAnimationDuration);
        Vector3 direction = (Vector3)(Vector2)controller.Players[playerTarget].GetComponent<PlayerController>().playerBody.position - transform.position;
        Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f));
    }
}
