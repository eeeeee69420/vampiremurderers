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


    protected override IEnumerator ActivateWeapon()
    {
        FindTarget(); //dont remove this
        remainingCooldown += stats.cooldown;
        if (target != null)
        {
            for (int i = 0; i < stats.amount; i++)
            {
                FindTarget();
                Vector3 direction = target.transform.position - transform.position;
                spawnedObjects.Add(Instantiate(weaponData.projectile, transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f)));
                spawnedObjects[^1].GetComponent<ProjectileController>().stats = stats.Clone();
                spawnedObjects[^1].transform.localScale *= spawnedObjects[^1].GetComponent<ProjectileController>().stats.area;
                spawnedObjects[^1].GetComponent<ProjectileController>().player = true;
                spawnedObjects[^1].GetComponent<ProjectileController>().owner = gameObject;
                spawnedObjects[^1].GetComponent<ProjectileController>().statusConditions = weaponData.statusConditions;
                spawnedObjects[^1].GetComponent<ProjectileController>().element = weaponData.element;
                yield return new WaitForSeconds(.1f);
            }
        }
    }
}
