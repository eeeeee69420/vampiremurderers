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
        if (target != null || weaponData.targetting == TargettingType.None)
        {
            for (int i = 0; i < stats.amount; i++)
            {
                float baseAngle = 90f;

                switch (weaponData.projectileFiringType)
                {
                    case ProjectileFiringType.None:
                        baseAngle = 90f;
                        break;
                    case ProjectileFiringType.Burst:
                        FindTarget();
                        baseAngle = 90f;
                        break;
                    case ProjectileFiringType.Spread:
                        baseAngle = 90 + weaponData.amountAngle * i;
                        break;
                    case ProjectileFiringType.AlternatingSpread:
                        int sign = (i % 2 == 0) ? 1 : -1;
                        int magnitude = (i + 1) / 2;
                        baseAngle = 90 + weaponData.amountAngle * sign * magnitude;
                        break;
                    case ProjectileFiringType.Omnidirectional:
                        baseAngle = 90 + i * 360 / stats.amount;
                        break;
                }

                for (int k = 0; k < weaponData.spreadAmount + 1; k++)
                {
                    float angle = baseAngle;

                    switch (weaponData.projectileSpreadType)
                    {
                        case ProjectileFiringType.Spread:
                            angle += weaponData.spreadAngle * k;
                            break;
                        case ProjectileFiringType.AlternatingSpread:
                            int sign = (k % 2 == 0) ? 1 : -1;
                            int magnitude = (k + 1) / 2;
                            angle += weaponData.spreadAngle * sign * magnitude;
                            break;
                        case ProjectileFiringType.Omnidirectional:
                            angle += k * 360 / (weaponData.spreadAmount + 1);
                            break;
                        default:
                            if (k > 0)
                                Debug.LogError("Spread type set improperly");
                            break;
                    }

                    Vector3 direction = target == null ? Vector2.zero : target.transform.position - transform.position;

                    GameObject proj = ProjectileManager.Instance.InstantiateProjectile(
                        weaponData.projectile,
                        transform.position,
                        Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - angle),
                        target == null ? null : target.gameObject,
                        gameObject.GetComponent<CharacterController>(),
                        stats,
                        gameObject.layer == 6,
                        weaponData.statusConditions,
                        weaponData.element
                    );
                    spawnedObjects.Add(proj);
                }

                yield return new WaitForSeconds(weaponData.amountDelay);
            }
        }
    }
}