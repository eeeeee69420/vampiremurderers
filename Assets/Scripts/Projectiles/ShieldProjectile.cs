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
        if (collision.gameObject.layer == 8)
        {
            collision.gameObject.GetComponent<EnemyBase>().enemyData.stats.hp -= stats.damage;
            collision.gameObject.GetComponent<EnemyBase>().Hit();
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * stats.projectileSpeed;
            collision.gameObject.GetComponent<EnemyBase>().freezeTimer = freezeTimer;
            owner.GetComponent<PlayerController>().LifeSteal(stats.lifesteal, stats.damage);
        }
        else if (collision.gameObject.layer == 9 && !collision.gameObject.GetComponent<ProjectileController>().player)
        {
            collision.gameObject.GetComponent<ProjectileController>().Despawn();
        }
    }
    protected virtual void OnTriggerStay2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            collision.gameObject.GetComponent<EnemyBase>().enemyData.stats.hp -= stats.damage * Time.deltaTime * 2;
            collision.gameObject.GetComponent<EnemyBase>().Hit();
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * stats.projectileSpeed;
            collision.gameObject.GetComponent<EnemyBase>().freezeTimer = freezeTimer;
            owner.GetComponent<PlayerController>().LifeSteal(stats.lifesteal, stats.damage * Time.deltaTime * 2);
        }
    }
}
