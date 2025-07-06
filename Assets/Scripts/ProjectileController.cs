using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed;
    public Rigidbody2D projectileBody;
    public new Transform transform;
    public Animator animator;
    public bool player;
    public float despawnTime;
    void Start()
    {
        projectileBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        transform = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        Move();
        despawnTime -= Time.fixedDeltaTime;
        if (despawnTime < 0)
            Despawn();
    }
    protected virtual void Move()
    {
        projectileBody.position += (Vector2)(speed * Time.fixedDeltaTime * transform.up);
    }
    protected virtual void Despawn()
    {
        Destroy(gameObject);
    }
    protected virtual void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
            GameController.Instance.hitScreenAnim();
    }
}
