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
            HitEnemy(collidedObject, stats.damage);
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
            HitEnemy(collidedObject, stats.damage * Time.deltaTime * 2);
            hitObjects.Remove(collidedObject);
        }
    }
    IEnumerator MarkUnhit(GameObject gameObject)
    {
        yield return new WaitForSeconds(.5f);
        hitObjects.Remove(gameObject);
    }
}
