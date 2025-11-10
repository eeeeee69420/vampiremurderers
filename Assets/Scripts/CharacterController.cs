using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D body;
    [HideInInspector] public SpriteRenderer sprite;
    [HideInInspector] public CharacterAnimator characterAnimator;

    [HideInInspector] public float hp;
    [HideInInspector] public CharacterStats stats = new();
    [HideInInspector] public CharacterStats buffs = new();
    [HideInInspector] public List<Weapon> Weapons;
    public List<StatusCondition> statusConditions = new();

    [HideInInspector] public Vector2 direction = new();

    [HideInInspector] public List<StatusStates> statusStates = new();
    [HideInInspector] public bool dead;

    public void Start()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        characterAnimator = GetComponent<CharacterAnimator>();
        RefreshStats();
    }
    public virtual void FixedUpdate()
    {
        if (!dead && !statusStates.Contains(StatusStates.Immobilized))
            Move();
        UpdateStatuses();
    }
    protected virtual Vector2 Track()
    {
        return new();
    }
    protected virtual void Move()
    {
        Vector2 direction = Track();
        if (direction.x < 0)
            sprite.flipX = true;
        else if (direction.x > 0)
            sprite.flipX = false;
        body.MovePosition(body.position + stats.moveSpeed * Time.fixedDeltaTime * direction);
    }
    public void LifeSteal()
    {
        int lifesteal = UnityEngine.Random.Range(1, 100);
        if (lifesteal <= stats.lifesteal)
        {
            hp += 1;
        }
    }
    public virtual void TakeDamage(float damage, ElementType element = ElementType.Typeless)
    {
        hp -= (damage - stats.armor);
        if (hp < 0 && stats.revives == 0)
        {
            StartCoroutine(Death());
        }
        else if (stats.revives > 0)
        {
            hp = stats.hpmax;
        }
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
                foreach (var state in statusCondition.states)
                {
                    statusStates.Remove(state);
                }
                statusConditions.RemoveAt(i);
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
        for (int i = 0; i < statusConditions.Count; i++)
        {
            for (int j = 0; j < statusConditions[i].statEffectors.Count; j++)
            {
                buffs.MergeBuff(statusConditions[i].statEffectors[j].currentAmount, statusConditions[i].statEffectors[j].affectedStat);
            }
        }
        RefreshStats();
    }
    public IEnumerator Death()
    {
        dead = true;
        hp = 0;
        characterAnimator.PlayAnimation("Death");
        yield return new WaitForSeconds(2f);
        if (gameObject.layer == 6)
            SceneManager.LoadScene("TestLevel");
        Destroy(gameObject);
    }
    public IEnumerator AddStatus(StatusCondition statusCondition)
    {
        yield return new WaitForSeconds(statusCondition.delay);

        var duplicates = statusConditions
            .Where(s => s.name == statusCondition.name)
            .ToList();

        if (duplicates.Count > statusCondition.maxStacks)
        {
            StatusCondition toRemove = duplicates.OrderBy(s => s.remainingDuration).First();
            statusConditions.Remove(toRemove);
            foreach (var state in toRemove.states)
            {
                statusStates.Remove(state);
            }
        }
        statusConditions.Add(statusCondition);
        statusConditions.Sort((a, b) => a.remainingDuration.CompareTo(b.remainingDuration));
        foreach (var state in statusCondition.states)
        {
            statusStates.Add(state);
        }
        if (statusCondition.damageOverTimes.Count > 0)
        {
            StartCoroutine(RunDOT(statusCondition));
        }
    }
    public IEnumerator RunDOT(StatusCondition statusCondition)
    {
        if (statusCondition.remainingDuration > 0)
        {
            foreach (var DOT in statusCondition.damageOverTimes)
            {
                TakeDamage(DOT.DPS, DOT.element);
            }
            yield return new WaitForSeconds(1f);
            StartCoroutine(RunDOT(statusCondition));
        }
    }
}
