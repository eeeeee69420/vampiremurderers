using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Game/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public WeaponStats baseStats;
    public List<List<StatIncrease>> LevelStats;
    public int maxLevel;
    public Sprite icon;
    public weaponBehaviors weaponBehavior;
    public targetting targetting;
    public GameObject projectile;
}
