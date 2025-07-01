using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    public float attackCooldown;
    public float remainingCooldown;
    public float range;
    public EnemyController enemyController;
    public GameObject projectile;
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
            if (enemyController.closestDistance <= range)
                StartCoroutine(ThrowProjectile());
        }
    }
    IEnumerator ThrowProjectile()
    {
        enemyController.enemyAnimator.animator.SetTrigger("IsThrowing");
        remainingCooldown = attackCooldown;
        yield return new WaitForSeconds(.3f);
        Vector3 direction = (Vector3)(Vector2)enemyController.controller.Players[enemyController.playerTarget].GetComponent<PlayerController>().playerBody.position - transform.position;
        Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f)); 
    }
}
