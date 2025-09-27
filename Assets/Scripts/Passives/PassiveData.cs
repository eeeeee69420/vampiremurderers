using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewPassive", menuName = "Game/Passive")]
public class PassiveData : ScriptableObject
{
    public string passiveName;
    public Sprite icon;
    public string description;

    public StatType affectedStat;
    public float bonusPerLevel;
    public int maxLevel;
}