using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : CharacterController
{
    [HideInInspector] public float closestDistance;
    [HideInInspector] public int playerTarget;
    [HideInInspector] public Vector2 targetPosition;

    [HideInInspector] public float remainingCooldown;
    [HideInInspector] public float attackAnimationDuration;
    public EnemyData enemyData;
    [HideInInspector] public float freezeTimer;
    public new void Start()
    {
        base.Start();
        characterAnimator.animator.runtimeAnimatorController = EnemyBehaviors.behaviorMap[enemyData.behavior].controller;
        characterAnimator.characterController = this;
        hp = enemyData.stats.hpmax;
        statusEffectGrid = GetComponentInChildren<GridLayoutGroup>().transform;
        statusEffectPrefab = enemyData.statusEffectPrefab;
        foreach (var clip in EnemyBehaviors.behaviorMap[enemyData.behavior].controller.animationClips)
        {
            if (clip.name == "Attack")
            {
                attackAnimationDuration = clip.length / 2;
            }
        }
    }
    public override Vector2 Track()
    {
        closestDistance = Mathf.Infinity;
        for (int i = 0; i < GameController.Instance.Players.Count; i++)
        {
            float dist = Vector2.Distance(body.position, GameController.Instance.Players[i].GetComponent<PlayerController>().body.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                playerTarget = i;
            }
        }
        targetPosition = GameController.Instance.Players[playerTarget].GetComponent<PlayerController>().body.position;
        return (targetPosition - body.position).normalized;
    }
    public virtual void Initialize()
    {
    }
    public override void RefreshStats()
    {
        stats = enemyData.stats.Clone();
        stats.ApplyBuffs(buffs);
    }
    public override void RefreshStatusDisplay(StatusCondition statusCondition, bool beingRemoved)
    {
        int stackCount = statusConditions.Count(s => s.displayName == statusCondition.displayName);
        Transform existingIcon = null;
        foreach (Transform icon in statusEffectGrid.transform)
        {
            if (icon.name.StartsWith(statusCondition.displayName))
            {
                existingIcon = icon;
                break;
            }
        }

        if (beingRemoved)
        {
            if (stackCount == 0 && existingIcon != null)
            {
                Destroy(existingIcon.gameObject);
            }
        }
        else
        {
            if (existingIcon == null)
            {
                GameObject newIcon = Instantiate(statusEffectPrefab, statusEffectGrid);
                newIcon.GetComponent<SpriteRenderer>().sprite = statusCondition.icon;
                newIcon.name = $"{statusCondition.displayName}_0";
            }
        }
    }
}
