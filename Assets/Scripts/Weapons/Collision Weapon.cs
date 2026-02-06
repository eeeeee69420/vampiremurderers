using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionWeapon : Weapon
{
    public override void FixedUpdate() { }

    protected virtual void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & enemyLayer.value) != 0)
        {
            if (!hitObjects.Contains(collision.gameObject))
            {
                hitObjects.Add(collision.gameObject);
                StartCoroutine(ActivateWeapon(collision.gameObject));
            }
        }
    }

    protected override IEnumerator ActivateWeapon(GameObject hitEnemy)
    {
        if (hitEnemy == null) yield break;
        if (weaponData.triggersAttackAnim)
        {
            controller.characterAnimator.PlayAnimation("Attack");
        }
        yield return new WaitForSeconds(stats.cooldown);
        if (hitEnemy != null)
        {
            IWeaponHit.HitEnemy(
                hitEnemy,
                stats.damage,
                gameObject.GetComponent<CharacterController>(),
                weaponData.element,
                weaponData.statusConditions,
                stats.projectileSpeed,
                KnockbackType.Radial,
                transform.position
            );
        }
        yield return new WaitForSeconds(0.5f);

        if (hitEnemy != null) hitObjects.Remove(hitEnemy);
        else hitObjects.RemoveAll(item => item == null);
    }
}