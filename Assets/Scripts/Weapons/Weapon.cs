using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
public class Weapon : MonoBehaviour
{
    [HideInInspector] public float remainingCooldown;
    [HideInInspector] public float range;
    public WeaponData weaponData;
    [HideInInspector] public CharacterStats stats = new();
    [HideInInspector] public int level;

    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public Type StatType = typeof(CharacterStats);

    public virtual void Initiate()
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
        if (weaponData.targetting != TargettingType.None)
            FindTarget();
        remainingCooldown += stats.cooldown;
        yield return new WaitForSeconds(.1f);
    }
    protected virtual void FindTarget()
    {

    }
    public virtual void LevelWeapon()
    {
        level++;
        RefreshStats();
    }
    public virtual void RefreshStats()
    {
        stats = weaponData.baseStats.Clone();
        for (int i = 0; i < level; i++)
        {
            foreach (var statIncrease in weaponData.LevelStats[i].statIncreases)
            {
                stats.ApplyBuff(statIncrease.stat, statIncrease.amount);
            }
        }
        stats = stats.ApplyBuffs(playerController.buffs);
        range = stats.duration * stats.projectileSpeed;
    }
}
