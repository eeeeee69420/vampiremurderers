using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class CollisionWeapon : Weapon
{

    protected virtual void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        IWeaponHit.HitEnemy(collision.gameObject, stats.damage, gameObject, weaponData.statusConditions, stats.projectileSpeed, KnockbackType.Radial, transform.position);
    }
    protected virtual void OnCollisionExit2D(UnityEngine.Collision2D collision)
    {
        hitObjects.Remove(collision.gameObject);
    }
}
