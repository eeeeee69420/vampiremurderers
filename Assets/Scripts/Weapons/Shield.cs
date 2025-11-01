using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : ProjectileWeapon
{
    GameObject shield;
    public float holdDistance = 1f;
    float angle;
    public float rotationSpeed = 180f;

    public override void Initiate()
    {
        playerController = GetComponent<PlayerController>();
        playerController.UpdateWeapons();
        shield = Instantiate(weaponData.projectile, playerController.transform);
        RefreshStats();

        shield.GetComponentInChildren<ShieldProjectile>().owner = this.gameObject;
    }
    protected override IEnumerator ActivateWeapon()
    {
        Vector2 dir = playerController.inputDirection;
        if (dir.magnitude > 0)
        {
            angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            shield.transform.rotation = Quaternion.RotateTowards(shield.transform.rotation, Quaternion.Euler(0f, 0f, angle - 90f), rotationSpeed * Time.fixedDeltaTime);
        }
        yield return null;
    }
    public override void RefreshStats()
    {
        buffStats = weaponData.baseStats.ApplyBuffs(playerController.stats);
        range = buffStats.projectileSpeed * buffStats.duration;
        shield.GetComponentInChildren<ShieldProjectile>().stats = buffStats.Clone();
        shield.transform.localScale *= shield.GetComponentInChildren<ProjectileController>().stats.area;
    }
}
