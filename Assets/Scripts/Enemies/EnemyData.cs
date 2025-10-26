using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Game/Enemy")]
public class EnemyData : ScriptableObject
{
    public string enemyName;

    public GameObject projectile;
    public CharacterStats stats;
    public EnemyBehavior behavior;
}