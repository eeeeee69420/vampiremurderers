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
            collision.gameObject.GetComponent<PlayerController>().hp -= stats.damage;
            GameController.Instance.HitScreenAnim();
            GameController.Instance.UpdateHPBar();
            Pierce();
            hitObjects.Add(collision.gameObject);
        }
        else if (player && collision.gameObject.layer == 8 && !hitObjects.Contains(collision.gameObject))
        {
            collision.gameObject.GetComponent<EnemyBase>().hp -= stats.damage;
            collision.gameObject.GetComponent<EnemyBase>().Hit();
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * stats.projectileSpeed;
            collision.gameObject.GetComponent<EnemyBase>().freezeTimer = freezeTimer;
            owner.GetComponent<PlayerController>().LifeSteal();
            Pierce();
            hitObjects.Add(collision.gameObject);
        }
    }
}
