using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class CollisionWeapon : Weapon
{
    public override void FixedUpdate()
    {
        
    }
    protected virtual void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & enemyLayer.value) != 0 && !hitObjects.Contains(collision.gameObject))
        {
            StartCoroutine(ActivateWeapon(collision.gameObject));
            hitObjects.Add(collision.gameObject);
        }
    }
    protected virtual void OnCollisionExit2D(UnityEngine.Collision2D collision)
    {
        hitObjects.Remove(collision.gameObject);
    }
    protected override IEnumerator ActivateWeapon(GameObject hitEnemy = null)
    {
        yield return new WaitForSeconds(stats.cooldown);
        IWeaponHit.HitEnemy(hitEnemy, stats.damage * stats.moveSpeed, gameObject, weaponData.statusConditions, stats.projectileSpeed, KnockbackType.Radial, transform.position);
    }
}
