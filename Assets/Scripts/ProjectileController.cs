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
    void Start()
    {
        projectileBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        transform = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        projectileBody.position += (Vector2)(speed * Time.fixedDeltaTime * transform.up);
    }
    public void RotateProjectile()
    {

    }
}
