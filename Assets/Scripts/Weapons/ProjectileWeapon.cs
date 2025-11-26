using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ProjectileWeapon : Weapon
{
    [HideInInspector] public List<GameObject> spawnedObjects = new();
    [HideInInspector] public float firingAngle = 90f;


    protected override IEnumerator ActivateWeapon(GameObject hitEnemy = null)
    {
        FindTarget();
        remainingCooldown += stats.cooldown;
        if (target != null)
        {
            for (int i = 0; i < stats.amount; i++)
            {
                switch (weaponData.projectileFiringType)
                {
                    case ProjectileFiringType.None:
                        Debug.LogError("projectile firing type not set");
                        break;
                    case ProjectileFiringType.Burst:
                        FindTarget();
                        firingAngle = 90f;
                        break;
                    case ProjectileFiringType.Spread:
                        int sign = (i % 2 == 0) ? 1 : -1;
                        int magnitude = (i + 1) / 2;
                        firingAngle = 90 + 10 * sign * magnitude;
                        break;
                }
                Vector3 direction = target.transform.position - transform.position;

                GameObject proj = ProjectileManager.Instance.InstantiateProjectile(
                    weaponData.projectile, 
                    transform.position, 
                    Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - firingAngle), 
                    target.gameObject, 
                    gameObject.GetComponent<CharacterController>(), 
                    stats, 
                    gameObject.layer == 6,
                    weaponData.statusConditions,
                    weaponData.element
                    );
                spawnedObjects.Add(proj);
                yield return new WaitForSeconds(.1f);
            }
        }
    }
}