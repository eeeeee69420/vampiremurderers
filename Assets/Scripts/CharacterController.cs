using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.ShaderData;
using static UnityEngine.EventSystems.EventTrigger;

public class CharacterController : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D body;
    [HideInInspector] public SpriteRenderer sprite;
    [HideInInspector] public CharacterAnimator characterAnimator;

    [HideInInspector] public float hp;
    [HideInInspector] public CharacterStats stats = new();
    [HideInInspector] public CharacterStats buffs = new();
    [HideInInspector] public List<Weapon> weapons = new();
    [HideInInspector] public float mass;
    [HideInInspector] public int revivesUsed;

    [HideInInspector] public List<StatusCondition> statusConditions = new();
    [HideInInspector] public List<StatusStates> statusStates = new();
    [HideInInspector] public bool dead;
    public Transform statusEffectGrid;
    public GameObject statusEffectPrefab;

    [HideInInspector] public Vector2 direction;

    public virtual void Start()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        characterAnimator = GetComponent<CharacterAnimator>();
        RefreshStats();
        mass = body.mass;
    }

    public virtual void FixedUpdate()
    {
        if (!dead && !statusStates.Contains(StatusStates.Immobilized))
        {
            Move();
        }
        UpdateStatuses();
    }
    public virtual void Update()
    {
    }
    public virtual Vector2 Track()
    {
        return new();
    }
    protected virtual void Move()
    {
        if (statusStates.Contains(StatusStates.Unstoppable))
        {
            float turnRate = 24f;
            Vector2 targetDir = Track();
            float currentAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float targetAngle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
            float newAngle = Mathf.MoveTowardsAngle(
                currentAngle,
                targetAngle,
                turnRate * Time.fixedDeltaTime
            );
            direction = new Vector2(Mathf.Cos(newAngle * Mathf.Deg2Rad),
                                    Mathf.Sin(newAngle * Mathf.Deg2Rad));
        }
        else
        {
            direction = Track();
        }
        if (direction.x < 0)
            sprite.flipX = true;
        else if (direction.x > 0)
            sprite.flipX = false;
        body.MovePosition(body.position + stats.moveSpeed * Time.fixedDeltaTime * direction);
    }
    public void LifeSteal()
    {
        float lifesteal = UnityEngine.Random.Range(0f, 1f);
        if (lifesteal <= stats.lifesteal)
        {
            hp += 1;
        }
    }
    public virtual void TakeDamage(float damage, ElementType element = ElementType.Typeless, bool ignoreArmor = false)
    {
        if (!ignoreArmor)
            hp -= damage - stats.armor;
        else
            hp -= damage;
        if (!dead && damage >= 1)
            GameController.Instance.ShowDamage(damage, element, transform.position);
        if (hp < 0 && stats.revives == 0)
            StartCoroutine(Death());
        else if (stats.revives > 0)
            hp = stats.hpmax;
    }
    public virtual void UpdateStatuses()
    {
        for (int i = statusConditions.Count - 1; i >= 0; i--)
        {
            var statusCondition = statusConditions[i];
            statusCondition.remainingDuration -= Time.fixedDeltaTime;
            foreach (var statEffector in statusCondition.statEffectors)
            {
                statEffector.SetCurrentAmount(statusCondition.remainingDuration, statusCondition.duration);
            }
            if (statusCondition.remainingDuration <= 0)
            {
                RemoveStatus(statusConditions[i]);
            }
        }
        RefreshBuffs();
    }
    public virtual void RefreshStats()
    {

    }
    public virtual void RefreshBuffs()
    {
        buffs = new();
        foreach (var status in statusConditions)
        {
            foreach (var statEffect in status.statEffectors)
            {
                buffs.MergeBuff(statEffect.currentAmount, statEffect.affectedStat);
            }
        }
        RefreshStats();
    }
    public IEnumerator Death()
    {
        var layersToExclude = new List<int> { 6, 8, 9 };
        GetComponent<CircleCollider2D>().excludeLayers = layersToExclude.Aggregate(0, (acc, layer) => acc | (1 << layer));
        foreach (var weapon in GetComponentsInChildren<Weapon>())
        { Destroy(weapon); }
        dead = true;
        hp = 0;
        characterAnimator.PlayAnimation("Death");
        foreach (var weapon in weapons)
            weapon.disabled = true;
        yield return new WaitForSeconds(2f);
        if (gameObject.layer == 6)
            SceneManager.LoadScene("StartMenu");
        if (gameObject.layer == 8)
            EnemyManager.Instance.DisableEnemy(gameObject);
    }
    public IEnumerator AddStatus(StatusCondition status, CharacterController owner = null)
    {
        StatusCondition statusCondition = status.Clone();
        if (owner != null)
        {
            statusCondition.owner = owner;
            statusCondition.remainingDuration = statusCondition.duration * (owner.stats.duration + 1);
        }
        else
            statusCondition.remainingDuration = statusCondition.duration;

        var duplicates = statusConditions
            .Where(s => s.name == statusCondition.name)
            .ToList();

        if (duplicates.Count > statusCondition.maxStacks)
        {
            StatusCondition toRemove = duplicates.OrderBy(s => s.remainingDuration).First();
            RemoveStatus(toRemove);
        }
        statusConditions.Add(statusCondition);
        statusConditions.Sort((a, b) => a.remainingDuration.CompareTo(b.remainingDuration));
        foreach (var state in statusCondition.states)
        {
            statusStates.Add(state);
            if (state == StatusStates.Immovable)
                body.mass = 100;
        }
        if (statusCondition.damageOverTimes.Count > 0)
        {
            StartCoroutine(RunDOT(statusCondition));
        }
        RefreshStatusDisplay(status, false);
        yield return null;
    }
    public IEnumerator RunDOT(StatusCondition statusCondition)
    {
        if (statusCondition.remainingDuration > 0)
        {
            foreach (var DOT in statusCondition.damageOverTimes)
            {
                switch (DOT.element)
                {
                    case ElementType.Typeless:
                        TakeDamage(DOT.DPS, DOT.element);
                        break;
                    case ElementType.Water:
                        TakeDamage(DOT.DPS * stats.hpmax, DOT.element);
                        break;
                    case ElementType.Fire:
                        TakeDamage(DOT.DPS * stats.hpmax, DOT.element, true);
                        break;
                    case ElementType.Grass:
                        TakeDamage(DOT.DPS, DOT.element);
                        statusCondition.owner.LifeSteal();
                        break;
                    case ElementType.Earth:
                        TakeDamage(DOT.DPS, DOT.element, true);
                        break;
                    case ElementType.Thunder:
                        TakeDamage(DOT.DPS, DOT.element);
                        StartCoroutine(AddStatus(GameController.Instance.electrifiedEffect));
                        break;
                    case ElementType.Air:
                        TakeDamage(DOT.DPS * (statusCondition.duration - statusCondition.remainingDuration), DOT.element);
                        break;
                    case ElementType.Ice:
                        TakeDamage(DOT.DPS, DOT.element);
                        break;
                    case ElementType.Poison:
                        TakeDamage(DOT.DPS, DOT.element);
                        break;
                    case ElementType.Light:
                        TakeDamage(DOT.DPS * statusCondition.owner.stats.damage, DOT.element);
                        break;
                    case ElementType.Dark:
                        TakeDamage(DOT.DPS * (stats.hpmax - hp), DOT.element);
                        break;
                }
            }
            yield return new WaitForSeconds(1f);
            StartCoroutine(RunDOT(statusCondition));
        }
    }
    public void RemoveStatus(StatusCondition statusCondition, bool runSequential = true)
    {
        if (statusCondition.sequentialEffect != null && runSequential)
            StartCoroutine(AddStatus(statusCondition.sequentialEffect, statusCondition.owner));
        foreach (var state in statusCondition.states)
        {
            statusStates.Remove(state);
            if (state != StatusStates.Immovable || runSequential && statusCondition.sequentialEffect?.states?.Contains(StatusStates.Immovable) == true)
                continue;
            body.mass = mass;
        }
        statusConditions.Remove(statusCondition);
        RefreshStatusDisplay(statusCondition, true);
    }
    public virtual void RefreshStatusDisplay(StatusCondition statusCondition, bool beingRemoved)
    {

    }
    public virtual void AddWeapon(WeaponData weaponData, bool tempWeapon = false, bool disabled = false)
    {
        Type behaviorType = WeaponBehaviors.behaviorMap[weaponData.weaponBehavior];
        Weapon newWeapon = (Weapon)gameObject.AddComponent(behaviorType);
        weapons.Add(newWeapon);
        newWeapon.weaponData = weaponData;
        newWeapon.Initiate();
        newWeapon.enemyLayer = LayerMask.GetMask("Player");
        if (tempWeapon)
            newWeapon.tempWeapon = true;
        if (disabled)
            newWeapon.disabled = true;
    }
    public void RemoveWeapon(Weapon weapon)
    {
        weapons.Remove(weapon);
        weapon.StopAllCoroutines();
        weapon.enabled = false;
        DestroyImmediate(weapon);
    }
}
