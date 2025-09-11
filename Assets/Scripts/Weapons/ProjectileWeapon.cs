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
        Debug.Log("start");
        for (int i = 0; i < buffStats.amount; i++)
        {
            Debug.Log("in");
            FindTarget();
            Vector3 direction = (Vector3)(Vector2)target.GetComponent<EnemyBase>().transform.position - target.transform.position;
            spawnedObjects.Add(Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f)));
            spawnedObjects[^1].GetComponent<ProjectileController>().stats = buffStats;
            gameObject.transform.localScale *= spawnedObjects[^1].GetComponent<ProjectileController>().stats.area;
            yield return new WaitForSeconds(.1f);
        }
    }
}
