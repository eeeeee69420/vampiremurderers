using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Game/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public List<List<StatIncrease>> LevelStats;
}
