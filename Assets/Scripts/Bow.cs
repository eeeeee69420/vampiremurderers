using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Bow : Weapon
{
    public GameObject projectile;
    protected override void FindTarget()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, range * buffStats.projectileSpeed * buffStats.duration, enemyLayer);
        float nearestDist = Mathf.Infinity;
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] == null) continue;
            float dist = Vector2.Distance(transform.position, targets[i].GetComponent<EnemyBase>().transform.position);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                target = targets[i];
            }
        }
    }
    protected override void ActivateWeapon()
    {
        FindTarget();
        remainingCooldown = cooldown / (1 + buffStats.cooldown);
        Vector3 direction = (Vector3)(Vector2)target.GetComponent<PlayerController>().transform.position - target.transform.position;
        Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f));
    }
}
