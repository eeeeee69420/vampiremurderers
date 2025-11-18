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
    public virtual void HitEnemy(GameObject collidedObject, float damage)
    {
        owner.GetComponent<CharacterController>().LifeSteal();
        collidedObject.GetComponent<CharacterController>().TakeDamage(damage);
        foreach (var status in statusConditions)
        {
            StartCoroutine(collidedObject.GetComponent<EnemyBase>().AddStatus(status, owner.GetComponent<CharacterController>()));
        }
        if (collidedObject.layer == 8)
            collidedObject.GetComponent<Rigidbody2D>().linearVelocity = stats.projectileSpeed * transform.up;
        hitObjects.Add(collidedObject);
        Pierce();
    }
    protected virtual void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (!player && collidedObject.layer == 6 && !hitObjects.Contains(collidedObject))
        {
            HitEnemy(collidedObject, stats.damage);
        }
        else if (player && collidedObject.layer == 8 && !hitObjects.Contains(collidedObject))
        {
            HitEnemy(collidedObject, stats.damage);
        }
    }
}
