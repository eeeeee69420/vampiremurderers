using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
public class Weapon : MonoBehaviour
{
    [HideInInspector] public float remainingCooldown;
    [HideInInspector] public float range;
    public WeaponData weaponData;
    [HideInInspector] public CharacterStats buffStats = new();
    [HideInInspector] public int level;

    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public Type StatType = typeof(CharacterStats);


    public virtual void Initiate()
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
        if (weaponData.targetting != TargettingType.None)
            FindTarget();
        remainingCooldown += buffStats.cooldown;
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
        buffStats = weaponData.baseStats.Clone();
        for (int i = 0; i < level; i++)
        {
            for (int k = 0; k < weaponData.LevelStats[i].Count; k++)
            {
                buffStats.ApplyBuff(weaponData.LevelStats[i][k].stat, weaponData.LevelStats[i][k].amount);
            }
        }
        buffStats = buffStats.ApplyBuffs(playerController.buffs);
    }
}
