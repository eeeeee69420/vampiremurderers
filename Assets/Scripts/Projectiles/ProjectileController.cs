using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public WeaponStats stats;
    public Rigidbody2D projectileBody;
    public Animator animator;
    public bool player;
    public float freezeTimer = .2f;
    public List<GameObject> hitObjects = new();
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
    protected virtual void Despawn()
    {
        Destroy(gameObject);
    }
    protected virtual void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {

        if (!player && collision.gameObject.layer == 6 && !hitObjects.Contains(collision.gameObject))
        {
            collision.gameObject.GetComponent<PlayerController>().stats.hp -= stats.damage;
            GameController.Instance.HitScreenAnim();
            GameController.Instance.UpdateHPBar();
            Pierce();
            hitObjects.Add(collision.gameObject);
        }
        else if (player && collision.gameObject.layer == 8 && !hitObjects.Contains(collision.gameObject))
        {
            collision.gameObject.GetComponent<EnemyBase>().hp -= stats.damage;
            collision.gameObject.GetComponent<EnemyBase>().hit();
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * stats.projectileSpeed;
            collision.gameObject.GetComponent<EnemyBase>().freezeTimer = freezeTimer;
            Pierce();
            Debug.Log(gameObject.name + "hit" + collision.gameObject.name);
            hitObjects.Add(collision.gameObject);
        }
    }
}
