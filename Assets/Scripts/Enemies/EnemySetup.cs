using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySetup : MonoBehaviour
{
    public EnemyData enemyData;
    public EnemyBase enemyBehavior;
    void Start()
    {
        
    }
    public void Initialize()
    {
        Type behaviorType = EnemyBehaviors.behaviorMap[enemyData.behavior];
        enemyBehavior = (EnemyBase)gameObject.AddComponent(behaviorType);
        enemyBehavior.enemyData = enemyData;
    }
}
