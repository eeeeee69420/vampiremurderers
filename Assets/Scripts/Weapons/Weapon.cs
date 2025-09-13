using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float remainingCooldown;
    public float range;
    public WeaponStats baseStats;
    public WeaponStats buffStats;

    public PlayerController playerController;
    public Sprite icon;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
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
        Debug.Log(baseStats.damage);
        Debug.Log(buffStats.damage);
        Debug.Log(playerController.stats.damage);
        buffStats.damage = baseStats.damage * playerController.stats.damage;
        buffStats.projectileSpeed = baseStats.projectileSpeed * playerController.stats.projectileSpeed;
        buffStats.cooldown = baseStats.cooldown / playerController.stats.cooldown;
        buffStats.area = baseStats.area * playerController.stats.area;
        buffStats.duration = baseStats.duration * playerController.stats.duration;
        buffStats.amount = baseStats.amount + playerController.stats.amount;
        buffStats.criticalChance = Mathf.Min(1f, baseStats.criticalChance + playerController.stats.criticalChance);
        buffStats.criticalDamage = baseStats.criticalDamage + playerController.stats.criticalDamage;
        buffStats.pierce = baseStats.pierce + playerController.stats.pierce;
        range = buffStats.projectileSpeed * buffStats.duration;
    }
}
