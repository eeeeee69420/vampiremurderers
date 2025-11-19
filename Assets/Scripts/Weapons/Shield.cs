using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : ProjectileWeapon
{
    GameObject shield;
    float angle;
    public float rotationSpeed = 180f;

    public override void Initiate()
    {
        playerController = GetComponent<PlayerController>();
        shield = Instantiate(weaponData.projectile, playerController.transform);
        RefreshStats();
        shield.GetComponentInChildren<ShieldProjectile>().statusConditions = weaponData.statusConditions;
        shield.GetComponentInChildren<ShieldProjectile>().owner = this.gameObject;
    }
    protected override IEnumerator ActivateWeapon()
    {
        Vector2 dir = playerController.direction;
        if (dir.magnitude > 0)
        {
            angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            shield.transform.rotation = Quaternion.RotateTowards(shield.transform.rotation, Quaternion.Euler(0f, 0f, angle - 90f), rotationSpeed * Time.fixedDeltaTime);
        }
        yield return null;
    }
    public override void RefreshStats()
    {
        base.RefreshStats();
        stats.projectileSpeed = weaponData.baseStats.projectileSpeed * playerController.stats.moveSpeed;
        shield.GetComponentInChildren<ShieldProjectile>().stats = stats.Clone();
        shield.transform.Find("Shield").localScale = new Vector3(shield.GetComponentInChildren<ProjectileController>().stats.area, shield.GetComponentInChildren<ProjectileController>().stats.area, 0);
        shield.GetComponentInChildren<ShieldProjectile>().statusConditions = weaponData.statusConditions;
    }
}
