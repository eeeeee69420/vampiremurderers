using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbilityData", menuName = "Game/Ability Data")]
public class AbilityData : ScriptableObject
{
    public string displayName;
    public Sprite icon;
    public float duration;
    public float cooldown;
    public List<StatusCondition> statusConditions;
    public List<WeaponData> tempWeapons;
    public List<AbilityEffects> abilityEffects;

}
