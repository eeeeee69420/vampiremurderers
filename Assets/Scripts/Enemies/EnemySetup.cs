using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class EnemySetup : MonoBehaviour
{
    public EnemyData enemyData;
    [HideInInspector] public EnemyBase enemyBehavior;

    void Start()
    {
        if (enemyData != null)
            Initialize();
    }

    public void Initialize()
    {              
        EnemyBase oldBehavior = GetComponent<EnemyBase>();
        if (oldBehavior != null)
        {
            Destroy(oldBehavior);
        }
        foreach (var weapon in GetComponents<Weapon>())
        {
            Destroy(weapon);
        }
        Type behaviorType = EnemyBehaviors.behavior[enemyData.behavior];
        gameObject.name = enemyData.name;
        enemyBehavior = (EnemyBase)gameObject.AddComponent(behaviorType);
        enemyBehavior.enemyData = enemyData;
        enemyBehavior.hp = enemyData.stats.hpmax;
        GetComponent<Collider2D>().enabled = true;
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
    }
}