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
        if (enemyData != null)
            Initialize();
    }
    public void Initialize()
    {
        Type behaviorType = EnemyBehaviors.behaviorMap[enemyData.behavior].behavior;
        enemyBehavior = (EnemyBase)gameObject.AddComponent(behaviorType);
        enemyBehavior.enemyData = enemyData;
        enemyBehavior.Initialize();
    }
}
