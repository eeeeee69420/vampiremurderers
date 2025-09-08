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

}
[System.Serializable]
public class PlayerStats
{
    public int level = 0;
    public float hpmax = 100;
    public float hp = 100;
    public float hpregen = 0;
    public int armor = 0;
    public float movespeed = 1;
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
}
[System.Serializable]
public class WeaponLevelStats
{
    public int level;
    public float damage;
    public float attackSpeed;
    public float range;
    public float cooldown;
    // add more as needed
}