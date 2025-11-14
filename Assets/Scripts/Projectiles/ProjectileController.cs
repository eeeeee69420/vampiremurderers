using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public CharacterStats stats;
    public Rigidbody2D projectileBody;
    public Animator animator;
    public bool player;
    public float freezeTimer = .2f;
    public List<GameObject> hitObjects = new();
    public GameObject owner;
    public List<StatusCondition> statusConditions;
    public ElementType element;
    void Start()
    {
        projectileBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Move();
        stats.duration -= Time.fixedDeltaTime;
        if (stats.duration < 0)
            Despawn();
    }
    protected virtual void Move()
    {
        projectileBody.position += (Vector2)(stats.projectileSpeed * Time.fixedDeltaTime * transform.up);
    }
    protected virtual void Pierce()
    {
        stats.pierce -= 1;
        if (stats.pierce == 0)
            Despawn();
    }
    public virtual void Despawn()
    {
        Destroy(gameObject);
    }
    public virtual void HitEnemy(Collider2D collision, float damage)
    {
        owner.GetComponent<CharacterController>().LifeSteal();
        collision.gameObject.GetComponent<CharacterController>().TakeDamage(damage);
        if (collision.gameObject.layer == 8)
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = stats.projectileSpeed * transform.up;
        foreach (var status in statusConditions)
        {
            StatusCondition statusClone = status.Clone();
            statusClone.remainingDuration = statusClone.duration * (owner.GetComponent<PlayerController>().buffs.duration + 1);
            if (statusClone.delayUsesDuration)
                statusClone.delay *= owner.GetComponent<PlayerController>().buffs.duration + 1;
            StartCoroutine(collision.gameObject.GetComponent<EnemyBase>().AddStatus(statusClone));
        }
        hitObjects.Add(collision.gameObject);
        Pierce();
    }
    protected virtual void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {

        if (!player && collision.gameObject.layer == 6 && !hitObjects.Contains(collision.gameObject))
        {
            HitEnemy(collision, stats.damage);
        }
        else if (player && collision.gameObject.layer == 8 && !hitObjects.Contains(collision.gameObject))
        {
            HitEnemy(collision, stats.damage);
        }
    }
}
