using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ProjectileWeapon : Weapon
{
    public GameObject projectile;
    public List<GameObject> spawnedObjects = new();
    public Collider2D target;
    public Collider2D[] targets;
    public LayerMask enemyMask;


    protected override IEnumerator ActivateWeapon()
    {
        remainingCooldown = buffStats.cooldown;
        for (int i = 0; i < buffStats.amount; i++)
        {
            FindTarget();
            Vector3 direction = target.transform.position - transform.position;
            spawnedObjects.Add(Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f)));
            spawnedObjects[^1].GetComponent<ProjectileController>().stats = buffStats.Clone();
            spawnedObjects[^1].transform.localScale *= spawnedObjects[^1].GetComponent<ProjectileController>().stats.area;
            spawnedObjects[^1].GetComponent<ProjectileController>().player = true;
            yield return new WaitForSeconds(.1f);
        }
    }
}
