using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

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
        GameObject collidedObject = collision.gameObject;
        if (!player && collidedObject.layer == 6 && !hitObjects.Contains(collidedObject))
        {
            IWeaponHit.HitEnemy(collidedObject, stats.damage, owner, element, statusConditions, stats.projectileSpeed, KnockbackType.Directional, transform.up);
            hitObjects.Add(collidedObject);
            Pierce();
        }
        else if (player && collidedObject.layer == 8 && !hitObjects.Contains(collidedObject))
        {
            IWeaponHit.HitEnemy(collidedObject, stats.damage, owner, element, statusConditions, stats.projectileSpeed, KnockbackType.Directional, transform.up);
            hitObjects.Add(collidedObject);
            Pierce();
        }
    }
}
