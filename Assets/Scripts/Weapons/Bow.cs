using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : ProjectileWeapon
{
    protected override void FindTarget()
    {
        targets = Physics2D.OverlapCircleAll(transform.position, buffStats.duration * buffStats.projectileSpeed * buffStats.duration, enemyMask);
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
}
