using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Database : MonoBehaviour
{

}

[System.Serializable]
public class WeaponStats
{
    public int level;
    public float damage;
    public float projectileSpeed;
    public float cooldown;
    public float area;
    public float duration;
    public float amount;
    public float criticalChance;
    public float criticalDamage;
    public int pierce;
    public WeaponStats Clone()
    {
        return new WeaponStats
        {
            level = this.level,
            damage = this.damage,
            projectileSpeed = this.projectileSpeed,
            cooldown = this.cooldown,
            area = this.area,
            duration = this.duration,
            amount = this.amount,
            criticalChance = this.criticalChance,
            criticalDamage = this.criticalDamage,
            pierce = this.pierce
        };
    }
}
[System.Serializable]
public class Passive
{
    public PassiveData data;
    public int level = 0;
}

[System.Serializable]
public class StatIncrease
{
    public StatType stat;
    public float amount;
}

[System.Serializable]
public class CharacterStats
{
    public float hpmax = 100;
    public float hpregen = 0;
    public int armor = 0;
    public float moveSpeed = 1;
    public float damage = 1;
    public float cooldown = 1;
    public float area = 1;
    public float duration = 1;
    public float projectileSpeed = 1;
    public int amount = 0;
    public float growth = 1;
    public float revives = 0;
    public float greed = 1;
    public float luck = 0;
    public float criticalChance = 0.15f;
    public float criticalDamage = 1.5f;
    public int pierce = 0;
    public float lifesteal = 0;
    public CharacterStats Clone()
    {
        return new CharacterStats
        {
            hpmax = this.hpmax,
            hpregen = this.hpregen,
            armor = this.armor,
            moveSpeed = this.moveSpeed,
            damage = this.damage,
            cooldown = this.cooldown,
            area = this.area,
            duration = this.duration,
            projectileSpeed = this.projectileSpeed,
            amount = this.amount,
            growth = this.growth,
            revives = this.revives,
            greed = this.greed,
            luck = this.luck,
            criticalChance = this.criticalChance,
            criticalDamage = this.criticalDamage,
            pierce = this.pierce,
            lifesteal = this.lifesteal
        };
    }
    public CharacterStats ApplyBuffs(CharacterStats buffs)
    {
        return new CharacterStats
        {
            hpmax = this.hpmax + buffs.hpmax,
            hpregen = this.hpregen + buffs.hpregen,
            armor = this.armor + buffs.armor,
            moveSpeed = this.moveSpeed * buffs.moveSpeed,
            damage = this.damage * buffs.damage,
            cooldown = this.cooldown / buffs.cooldown,
            area = this.area * buffs.area,
            duration = this.duration * buffs.duration,
            projectileSpeed = this.projectileSpeed * buffs.projectileSpeed,
            amount = this.amount + buffs.amount,
            growth = this.growth + buffs.growth,
            revives = this.revives + buffs.revives,
            greed = this.greed + buffs.greed,
            luck = this.luck + buffs.luck,
            criticalChance = this.criticalChance + buffs.criticalChance,
            criticalDamage = this.criticalDamage + buffs.criticalDamage,
            pierce = this.pierce + buffs.pierce,
            lifesteal = this.lifesteal + buffs.lifesteal
        };
    }
    public void ApplyBuff(StatType statType, float buffValue)
    {
        switch (statType)
        {
            case StatType.HpMax: hpmax += buffValue; break;
            case StatType.HpRegen: hpregen += buffValue; break;
            case StatType.Armor: armor += (int)buffValue; break;
            case StatType.MoveSpeed: moveSpeed *= buffValue; break;
            case StatType.Damage: damage *= buffValue; break;
            case StatType.Cooldown: cooldown /= buffValue; break;
            case StatType.Area: area *= buffValue; break;
            case StatType.Duration: duration *= buffValue; break;
            case StatType.ProjectileSpeed: projectileSpeed *= buffValue; break;
            case StatType.Amount: amount += (int)buffValue; break;
            case StatType.Growth: growth *= buffValue; break;
            case StatType.Revives: revives += (int)buffValue; break;
            case StatType.Greed: greed += buffValue; break;
            case StatType.Luck: luck += buffValue; break;
            case StatType.CriticalChance: criticalChance += buffValue; break;
            case StatType.CriticalDamage: criticalDamage += buffValue; break;
            case StatType.Pierce: pierce += (int)buffValue; break;
            case StatType.Lifesteal: lifesteal += buffValue; break;
        }
    }
    public CharacterStats MergeBuffs(CharacterStats buffs1, CharacterStats buffs2)
    {
        return new CharacterStats
        {
            hpmax = buffs1.hpmax + buffs2.hpmax,
            hpregen = buffs1.hpregen + buffs2.hpregen,
            armor = buffs1.armor + buffs2.armor,
            moveSpeed = buffs1.moveSpeed + buffs2.moveSpeed,
            damage = buffs1.damage + buffs2.damage,
            cooldown = buffs1.cooldown + buffs2.cooldown,
            area = buffs1.area + buffs2.area,
            duration = buffs1.duration + buffs2.duration,
            projectileSpeed = buffs1.projectileSpeed + buffs2.projectileSpeed,
            amount = buffs1.amount + buffs2.amount,
            growth = buffs1.growth + buffs2.growth,
            revives = buffs1.revives + buffs2.revives,
            greed = buffs1.greed + buffs2.greed,
            luck = buffs1.luck + buffs2.luck,
            criticalChance = buffs1.criticalChance + buffs2.criticalChance,
            criticalDamage = buffs1.criticalDamage + buffs2.criticalDamage,
            pierce = buffs1.pierce + buffs2.pierce,
            lifesteal = buffs1.lifesteal + buffs2.lifesteal
        };
    }
}
public enum StatType
{
    HpMax, //Flat
    HpRegen, //Flat
    Armor, //Flat
    MoveSpeed, //Additive
    Damage, //Additive
    Cooldown, //Inverse Additive
    Area, //Additive
    Duration, //Additive
    ProjectileSpeed, //Additive
    Amount, //Flat
    Growth, //Additive
    Revives, //Flat
    Greed, //Flat
    Luck, //Flat
    CriticalChance, //Flat
    CriticalDamage, //Flat
    Pierce, //Flat
    Lifesteal //Flat
}
public enum TargettingType
{
    None,
    Random,
    Closest,
    Farthest,
    Strongest,
    Weakest
}
public enum WeaponBehavior
{
    Shield,
    Projectile,
    SpreadProjectile,
    BurstProjectile,
    RadialProjectile,
    OrbittingProjectile,
    Aura
}
public static class WeaponBehaviors
{
    public static readonly Dictionary<WeaponBehavior, Type> behaviorMap = new()
    {
        { WeaponBehavior.Shield, typeof(Shield) },
        { WeaponBehavior.Projectile, typeof(ProjectileWeapon) },
    };
}
public enum ElementType
{
    Water,
    Fire,
    Grass,
    Earth,
    Thunder,
    Air,
    Ice,
    Poison,
    Light,
    Dark
}
public enum EnemyBehavior
{
    Melee,
    RangedHold
}
public static class EnemyBehaviors
{
    public static readonly Dictionary<EnemyBehavior, (Type behavior, RuntimeAnimatorController controller)> behaviorMap = new()
    {
        { EnemyBehavior.Melee, (typeof(EnemyBase), Resources.Load<RuntimeAnimatorController>("Controllers/MeleeEnemy")) },
        { EnemyBehavior.RangedHold, (typeof(EnemyRangedHold), Resources.Load<RuntimeAnimatorController>("Controllers/RangedEnemyHold")) },
    };
}