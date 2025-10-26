using System;
using System.Collections;
using System.Collections.Generic;
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
    public float lifesteal;
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
            pierce = this.pierce,
            lifesteal = this.lifesteal
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
    public int level = 0;
    public float hpmax = 100;
    public float hp = 100;
    public float hpregen = 0;
    public int armor = 0;
    public float moveSpeed = 5;
    public float damage = 1;
    public float cooldown = 1;
    public float area = 1;
    public float duration = 1;
    public float projectileSpeed = 1;
    public float amount = 0;
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
            level = this.level,
            hpmax = this.hpmax,
            hp = this.hp,
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
}
public enum StatType
{
    HpMax,
    HpRegen,
    Armor,
    MoveSpeed,
    Damage,
    Cooldown,
    Area,
    Duration,
    ProjectileSpeed,
    Amount,
    Growth,
    Revives,
    Greed,
    Luck,
    CriticalChance,
    CriticalDamage,
    Pierce,
    Lifesteal
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
    public static readonly Dictionary<EnemyBehavior, Type> behaviorMap = new()
    {
        { EnemyBehavior.Melee, typeof(EnemyBase) },
        { EnemyBehavior.RangedHold, typeof(EnemyRangedHold) },
    };
}
