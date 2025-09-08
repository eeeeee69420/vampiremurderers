using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public WeaponStats stats;
    public Rigidbody2D projectileBody;
    public new Transform transform;
    public Animator animator;
    public bool player;
    void Start()
    {
        projectileBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        transform = GetComponent<Transform>();
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
    protected virtual void Despawn()
    {
        Destroy(gameObject);
    }
    protected virtual void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (!player && collision.gameObject.layer == 6)
        {
            collision.gameObject.GetComponent<PlayerController>().stats.hp -= stats.damage;
            GameController.Instance.HitScreenAnim();
            GameController.Instance.UpdateHPBar();
        }
        if (player && collision.gameObject.layer == 7)
        {
            collision.gameObject.GetComponent<EnemyBase>().hp -= stats.damage;
            collision.gameObject.GetComponent<EnemyBase>().hit();
        }
    }
}
