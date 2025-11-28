using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Game/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public CharacterStats baseStats;
    public List<LevelStatIncrease> LevelStats;
    public Sprite icon;
    public WeaponBehavior weaponBehavior;
    public TargettingType targetting;
    public List<StatusCondition> statusConditions;
    public ElementType element;
    public ProjectileData projectile;
    public ProjectileFiringType projectileFiringType;
    public float radius;
    public float amountDelay;
}
