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
    protected virtual void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {

        if (!player && collision.gameObject.layer == 6 && !hitObjects.Contains(collision.gameObject))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(stats.damage, element);
            Pierce();
            hitObjects.Add(collision.gameObject);
            for (int i = 0; i < statusConditions.Count; i++)
            {
                StartCoroutine(collision.gameObject.GetComponent<EnemyBase>().AddStatus(statusConditions[i].Clone(owner.GetComponent<PlayerController>().buffs.duration)));
            }
        }
        else if (player && collision.gameObject.layer == 8 && !hitObjects.Contains(collision.gameObject))
        {
            collision.gameObject.GetComponent<EnemyBase>().TakeDamage(stats.damage, element);
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = transform.up * stats.projectileSpeed;
            owner.GetComponent<PlayerController>().LifeSteal();
            Pierce();
            hitObjects.Add(collision.gameObject);
            for (int i = 0; i < statusConditions.Count; i++)
            {
                StartCoroutine(collision.gameObject.GetComponent<EnemyBase>().AddStatus(statusConditions[i].Clone(owner.GetComponent<PlayerController>().buffs.duration)));
            }
        }
    }
}
