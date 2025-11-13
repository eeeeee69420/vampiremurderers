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
    protected override void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.layer == 8 && !hitObjects.Contains(collision.gameObject))
        {
            HitEnemy(collision, stats.damage);
            StartCoroutine(MarkUnhit(collision.gameObject));
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
            HitEnemy(collision, stats.damage * Time.deltaTime * 2);
        }
    }
    IEnumerator MarkUnhit(GameObject gameObject)
    {
        yield return new WaitForSeconds(.5f);
        hitObjects.Remove(gameObject);
    }
}
