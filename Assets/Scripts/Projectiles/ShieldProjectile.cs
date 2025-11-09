using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ShieldProjectile : ProjectileController
{
    public AudioClip soundEffect;
    public AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    protected override void Move()
    {
    }
    protected override void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            collision.gameObject.GetComponent<EnemyBase>().TakeDamage(stats.damage);
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = stats.moveSpeed * stats.projectileSpeed * transform.up;
            audioSource.PlayOneShot(soundEffect);
            for (int i = 0; i < statusConditions.Count; i++)
            {
                StartCoroutine(collision.gameObject.GetComponent<EnemyBase>().AddStatus(statusConditions[i]));
            }
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
            collision.gameObject.GetComponent<EnemyBase>().TakeDamage(stats.damage * Time.deltaTime * 2);
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = stats.moveSpeed * stats.projectileSpeed * transform.up;
            audioSource.PlayOneShot(soundEffect);
            for (int i = 0; i < statusConditions.Count; i++)
            {
                StatusCondition statusClone = statusConditions[i].Clone(owner.GetComponent<PlayerController>().buffs.duration);
                StartCoroutine(collision.gameObject.GetComponent<EnemyBase>().AddStatus(statusClone));
            }
        }
    }
}
