using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class CollisionWeapon : Weapon
{

    protected virtual void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (collision.gameObject.layer == 8 && !hitObjects.Contains(collision.gameObject))
        {
            IWeaponHit.HitEnemy(collision.gameObject, stats.damage * stats.moveSpeed, gameObject, weaponData.statusConditions, stats.projectileSpeed, KnockbackType.Radial, transform.position);
            hitObjects.Add(collision.gameObject);
        }
    }
    protected virtual void OnCollisionExit2D(UnityEngine.Collision2D collision)
    {
        hitObjects.Remove(collision.gameObject);
    }
}
