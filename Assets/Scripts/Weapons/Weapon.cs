using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem.XR;
public class Weapon : MonoBehaviour
{
    [HideInInspector] public float remainingCooldown;
    [HideInInspector] public float range;
    public WeaponData weaponData;
    [HideInInspector] public CharacterStats stats = new();
    [HideInInspector] public int level;

    [HideInInspector] public CharacterController controller;
    [HideInInspector] public Type StatType = typeof(CharacterStats);

    [HideInInspector] public Collider2D target;
    [HideInInspector] public Collider2D[] targets;
    public LayerMask enemyMask;
    public List<GameObject> hitObjects = new();


    public virtual void Initiate()
    {
        controller = GetComponent<CharacterController>();
        RefreshStats();
        enemyMask = LayerMask.GetMask("Enemy");
    }
    void FixedUpdate()
    {
        remainingCooldown -= Time.fixedDeltaTime;
        if (remainingCooldown <= 0)
            StartCoroutine(ActivateWeapon());
    }
    protected virtual IEnumerator ActivateWeapon()
    {
        if (weaponData.targetting != TargettingType.None)
            FindTarget();
        remainingCooldown += stats.cooldown;
        yield return new WaitForSeconds(.1f);
    }
    protected virtual void FindTarget()
    {
        targets = Physics2D.OverlapCircleAll(transform.position, range, enemyMask);
        List<Collider2D> targetList = new(
            Physics2D.OverlapCircleAll(transform.position, range, enemyMask)
        );

        for (int i = targetList.Count - 1; i >= 0; i--)
        {
            EnemyBase enemy = targetList[i].GetComponent<EnemyBase>();
            if (enemy != null && (enemy.dead || enemy.statusStates.Contains(StatusStates.Untargetable)))
                targetList.RemoveAt(i);
        }

        targets = targetList.ToArray();
        switch (weaponData.targetting)
        {
            case TargettingType.Closest:
                float nearestDist = Mathf.Infinity;
                for (int i = 0; i < targets.Length; i++)
                {
                    if (targets[i] == null) continue;
                    float dist = Vector2.Distance(transform.position, targets[i].GetComponent<EnemyBase>().transform.position);
                    if (dist < nearestDist)
                    {
                        nearestDist = dist;
                        target = targets[i];
                    }
                }
                break;
            case TargettingType.Farthest:
                float farthestDist = 0;
                for (int i = 0; i < targets.Length; i++)
                {
                    if (targets[i] == null) continue;
                    float dist = Vector2.Distance(transform.position, targets[i].GetComponent<EnemyBase>().transform.position);
                    if (dist > farthestDist)
                    {
                        farthestDist = dist;
                        target = targets[i];
                    }
                }
                break;
            case TargettingType.Random:
                int targetIndex = UnityEngine.Random.Range(0, targets.Length - 1);
                target = targets[targetIndex];
                break;
            case TargettingType.Weakest:
                targets = targets.OrderBy(collider => collider.GetComponent<EnemyBase>().hp).ToArray();
                target = targets[0];
                break;
            case TargettingType.Strongest:
                targets = targets.OrderBy(collider => collider.GetComponent<EnemyBase>().hp).ToArray();
                target = targets[0];
                break;
        }
    }
    public virtual void LevelWeapon()
    {
        level++;
        RefreshStats();
    }
    public virtual void RefreshStats()
    {
        stats = weaponData.baseStats.Clone();
        for (int i = 0; i < level; i++)
        {
            foreach (var statIncrease in weaponData.LevelStats[i].statIncreases)
            {
                stats.ApplyBuff(statIncrease.stat, statIncrease.amount);
            }
        }
        stats = stats.ApplyBuffs(controller.buffs);
        range = stats.duration * stats.projectileSpeed;
    }
    public virtual void HitEnemy(GameObject collidedObject, float damage, bool markHit = true)
    {
        GetComponent<CharacterController>().LifeSteal();
        collidedObject.GetComponent<CharacterController>().TakeDamage(damage);
        foreach (var status in weaponData.statusConditions)
        {
            StartCoroutine(collidedObject.GetComponent<EnemyBase>().AddStatus(status, GetComponent<CharacterController>()));
        }
        if (collidedObject.layer == 8)
            collidedObject.GetComponent<Rigidbody2D>().linearVelocity = stats.projectileSpeed * transform.up;
        if (markHit)
            hitObjects.Add(collidedObject);
    }
}
