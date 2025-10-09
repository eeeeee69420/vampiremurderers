using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Weapon : MonoBehaviour
{
    [HideInInspector] public float remainingCooldown;
    [HideInInspector] public float range;
    public WeaponData weaponData;
    [HideInInspector] public WeaponStats buffStats;

    [HideInInspector] public PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerController.UpdateWeapons();
        RefreshStats();
    }

    void FixedUpdate()
    {
        remainingCooldown -= Time.fixedDeltaTime;
        if (remainingCooldown <= 0)
            StartCoroutine(ActivateWeapon());
    }
    protected virtual IEnumerator ActivateWeapon()
    {
        if (weaponData.targetting != targetting.None)
            FindTarget();
        remainingCooldown = buffStats.cooldown;
        yield return new WaitForSeconds(.1f);
    }
    protected virtual void FindTarget()
    {

    }
    public virtual void LevelStats(int level)
    {

    }
    public virtual void RefreshStats()
    {
        buffStats.damage = weaponData.baseStats.damage * playerController.stats.damage;
        buffStats.projectileSpeed = weaponData.baseStats.projectileSpeed * playerController.stats.projectileSpeed;
        buffStats.cooldown = weaponData.baseStats.cooldown / playerController.stats.cooldown;
        buffStats.area = weaponData.baseStats.area * playerController.stats.area;
        buffStats.duration = weaponData.baseStats.duration * playerController.stats.duration;
        buffStats.amount = weaponData.baseStats.amount + playerController.stats.amount;
        buffStats.criticalChance = Mathf.Min(1f, weaponData.baseStats.criticalChance + playerController.stats.criticalChance);
        buffStats.criticalDamage = weaponData.baseStats.criticalDamage + playerController.stats.criticalDamage;
        buffStats.pierce = weaponData.baseStats.pierce + playerController.stats.pierce;
        range = buffStats.projectileSpeed * buffStats.duration;
    }
}
