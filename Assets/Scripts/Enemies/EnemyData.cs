using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Game/Enemy")]
public class EnemyData : ScriptableObject
{
    public string enemyName;

    public CharacterStats stats;
    public EnemyBehavior behavior;
    public RuntimeAnimatorController animationController;
    public ElementType element;
    public GameObject statusEffectPrefab;
    public List<WeaponData> weapons;
    public float preferredDistance;
}