using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : ScriptableObject
{
    public string enemyName;
    public Sprite icon;
    public string description;

    public string weapon;
    public string ability;
    public PlayerStats stats;
}