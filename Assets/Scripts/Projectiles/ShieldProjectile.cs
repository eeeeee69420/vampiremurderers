using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ShieldProjectile : ProjectileController
{
    protected override void Move()
    {
    }
    protected override void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (player && collision.gameObject.layer == 8)
        {
            collision.gameObject.GetComponent<EnemyBase>().hp -= stats.damage;
            collision.gameObject.GetComponent<EnemyBase>().hit();
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * stats.projectileSpeed;
            collision.gameObject.GetComponent<EnemyBase>().freezeTimer = freezeTimer;
        }
        else if (collision.gameObject.layer == 9 && !collision.gameObject.GetComponent<ProjectileController>().player)
        {
            collision.gameObject.GetComponent<ProjectileController>().Despawn();
        }
    }
}
