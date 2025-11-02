using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ProjectileWeapon : Weapon
{
    [HideInInspector] public List<GameObject> spawnedObjects = new();
    [HideInInspector] public Collider2D target;
    [HideInInspector] public Collider2D[] targets;
    public LayerMask enemyMask;
    private void Start()
    {
        enemyMask = LayerMask.GetMask("Enemy");
    }
    protected override void FindTarget()
    {
        targets = Physics2D.OverlapCircleAll(transform.position, range, enemyMask);
        switch (weaponData.targetting)
        {
            case TargettingType.Closest:
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
                break;
            case TargettingType.Farthest:
                float farthestDist = 0;
                for (int i = 0; i < targets.Length; i++)
                {
                    if (targets[i] == null) continue;
                    float dist = Vector2.Distance(transform.position, targets[i].GetComponent<EnemyBase>().transform.position);
                    if (dist > farthestDist)
                    {
                        farthestDist = dist;
                        target = targets[i];
                    }
                }
                break;
            case TargettingType.Random:
                int targetIndex = Random.Range(0, targets.Length - 1);
                target = targets[targetIndex];
                break;
            case TargettingType.Weakest:
                targets = targets.OrderBy(collider => collider.GetComponent<EnemyBase>().hp).ToArray();
                target = targets[0];
                break;
            case TargettingType.Strongest:
                targets = targets.OrderBy(collider => collider.GetComponent<EnemyBase>().hp).ToArray();
                target = targets[0];
                break;
        }
    }

    protected override IEnumerator ActivateWeapon()
    {
        FindTarget();
        remainingCooldown += buffStats.cooldown;
        if (target != null)
        {
            for (int i = 0; i < buffStats.amount; i++)
            {
                FindTarget();
                Vector3 direction = target.transform.position - transform.position;
                spawnedObjects.Add(Instantiate(weaponData.projectile, transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f)));
                spawnedObjects[^1].GetComponent<ProjectileController>().stats = buffStats.Clone();
                spawnedObjects[^1].transform.localScale *= spawnedObjects[^1].GetComponent<ProjectileController>().stats.area;
                spawnedObjects[^1].GetComponent<ProjectileController>().player = true;
                spawnedObjects[^1].GetComponent<ProjectileController>().owner = gameObject;
                yield return new WaitForSeconds(.1f);
            }
        }
    }
}
