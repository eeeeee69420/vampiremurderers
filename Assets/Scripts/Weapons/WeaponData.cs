using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Game/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public CharacterStats baseStats;
    public List<List<StatIncrease>> LevelStats;
    public int maxLevel;
    public Sprite icon;
    public WeaponBehavior weaponBehavior;
    public TargettingType targetting;
    public GameObject projectile;
}
