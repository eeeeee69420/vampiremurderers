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


    protected override IEnumerator ActivateWeapon(GameObject hitEnemy = null)
    {
        FindTarget();
        remainingCooldown += stats.cooldown;
        if (target != null)
        {
            for (int i = 0; i < stats.amount; i++)
            {
                FindTarget();
                Vector3 direction = target.transform.position - transform.position;

                var proj = Instantiate(weaponData.projectile, transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f));
                var controller = proj.GetComponent<ProjectileController>();
                controller.stats = stats.Clone();
                controller.player = true;
                controller.owner = gameObject;
                controller.statusConditions = weaponData.statusConditions;
                controller.element = weaponData.element;

                proj.transform.localScale *= controller.stats.area;
                spawnedObjects.Add(proj);

                yield return new WaitForSeconds(.1f);
            }
        }
    }
}