using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ShieldProjectile : ProjectileController
{
    public List<AudioClip> soundEffects;
    public AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    protected override void Move()
    {
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.layer == 8 && !hitObjects.Contains(collidedObject))
        {
            IWeaponHit.HitEnemy(collidedObject, stats.damage, owner, statusConditions, stats.projectileSpeed, KnockbackType.Radial, transform.position);
            hitObjects.Add(collidedObject);
            StartCoroutine(MarkUnhit(collidedObject));
        }
        else if (collidedObject.layer == 9 && !collidedObject.GetComponent<ProjectileController>().player)
        {
            collidedObject.GetComponent<ProjectileController>().Despawn();
        }
    }


    protected void OnCollisionStay2D(Collision2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.layer == 8)
        {
            IWeaponHit.HitEnemy(collidedObject, stats.damage * Time.deltaTime * 2, owner, statusConditions, stats.projectileSpeed, KnockbackType.Radial, transform.position);
        }
    }
    IEnumerator MarkUnhit(GameObject gameObject)
    {
        yield return new WaitForSeconds(.5f);
        hitObjects.Remove(gameObject);
    }
}
