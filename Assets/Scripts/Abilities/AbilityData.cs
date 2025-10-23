using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Game/Passive")]
public class AbilityData : ScriptableObject
{
    public string abilityName;
    public Sprite icon;
    public string description;
}