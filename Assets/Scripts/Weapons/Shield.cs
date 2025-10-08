using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Weapon
{
    public GameObject shield;
    public GameObject shieldPrefab;
    public float holdDistance;
    public float angle;
    public float rotationSpeed;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        shield = Instantiate(shieldPrefab, playerController.transform);
        RefreshStats();
    }
    protected override IEnumerator ActivateWeapon()
    {
        Vector2 dir = playerController.inputDirection;
        if (dir.magnitude > 0)
            angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        shield.transform.rotation = Quaternion.RotateTowards(shield.transform.rotation, Quaternion.Euler(0f, 0f, angle - 90f), rotationSpeed * Time.fixedDeltaTime);
        yield return null;
    }
    public override void RefreshStats()
    {
        buffStats.damage = weaponData.baseStats.damage * playerController.stats.damage * playerController.stats.moveSpeed;
        buffStats.projectileSpeed = weaponData.baseStats.projectileSpeed * playerController.stats.projectileSpeed;
        buffStats.cooldown = weaponData.baseStats.cooldown / playerController.stats.cooldown;
        buffStats.area = weaponData.baseStats.area * playerController.stats.area;
        buffStats.duration = weaponData.baseStats.duration * playerController.stats.duration;
        buffStats.amount = weaponData.baseStats.amount + playerController.stats.amount;
        buffStats.criticalChance = Mathf.Min(1f, weaponData.baseStats.criticalChance + playerController.stats.criticalChance);
        buffStats.criticalDamage = weaponData.baseStats.criticalDamage + playerController.stats.criticalDamage;
        buffStats.pierce = weaponData.baseStats.pierce + playerController.stats.pierce;
        range = buffStats.projectileSpeed * buffStats.duration;
        shield.GetComponentInChildren<ProjectileController>().stats = buffStats.Clone();
        shield.transform.localScale *= shield.GetComponentInChildren<ProjectileController>().stats.area;
    }
}
