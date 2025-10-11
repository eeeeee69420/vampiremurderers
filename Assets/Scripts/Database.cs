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
public class PlayerStats
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
    public PlayerStats Clone()
    {
        return new PlayerStats
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
            pierce = this.pierce
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
    Pierce
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
    SpreadProjectile,
    BurstProjectile,
    RadialProjectile,
    OrbittingProjectile
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
public static class ElementalChart
{
    // Attacker -> set of defender elements that attacker is strong against
    public static readonly Dictionary<ElementType, HashSet<ElementType>> StrongAgainst =
        new()
        {
            { ElementType.Fire,    new HashSet<ElementType> { ElementType.Grass, ElementType.Ice } },
            { ElementType.Water,   new HashSet<ElementType> { ElementType.Fire, ElementType.Earth } },
            { ElementType.Earth,   new HashSet<ElementType> { ElementType.Fire, ElementType.Thunder } },
            { ElementType.Thunder, new HashSet<ElementType> { ElementType.Water, ElementType.Ice } },
            { ElementType.Air,     new HashSet<ElementType> { ElementType.Earth, ElementType.Poison } },
            { ElementType.Ice,     new HashSet<ElementType> { ElementType.Air, ElementType.Grass } },
            { ElementType.Grass,   new HashSet<ElementType> { ElementType.Water, ElementType.Earth } },
            { ElementType.Poison,  new HashSet<ElementType> { ElementType.Grass, ElementType.Air } },
            { ElementType.Dark,    new HashSet<ElementType> { ElementType.Light } },
            { ElementType.Light,   new HashSet<ElementType> { ElementType.Dark } },
        };

    // Returns damage multiplier: 1 = normal, >1 = super effective, <1 = not effective
    public static float GetEffectiveness(ElementType attacker, ElementType defender)
    {
        if (attacker == defender) return 1f;

        // attacker strong against defender -> super effective
        if (StrongAgainst.TryGetValue(attacker, out var attackerSet) && attackerSet.Contains(defender))
            return 1.5f;

        // defender strong against attacker -> not effective
        if (StrongAgainst.TryGetValue(defender, out var defenderSet) && defenderSet.Contains(attacker))
            return 0.5f;

        return 1f;
    }
}
public enum enemyBehaviors
{
    Melee,
    Ranged
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
