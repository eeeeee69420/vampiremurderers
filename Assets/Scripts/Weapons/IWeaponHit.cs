using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class IWeaponHit : MonoBehaviour
{
    public static void HitEnemy(GameObject enemy, float damage, GameObject owner, ElementType element = ElementType.Typeless, List<StatusCondition> statusConditions = null, float knockback = 0, KnockbackType knockbackType = default, Vector2 origin = default)
    {
        var enemyController = enemy.GetComponent<CharacterController>();
        var ownerController = owner.GetComponent<CharacterController>();
        ownerController?.LifeSteal();
        enemyController?.TakeDamage(damage, element);

        foreach (var status in statusConditions)
        {
            if (statusConditions != null && enemyController != null && ownerController != null)
            {
                enemyController.StartCoroutine(enemyController.AddStatus(status, ownerController));
            }
        }
        if (knockback > 0)
            ApplyKnockback(enemy.GetComponent<Rigidbody2D>(), origin, knockback, knockbackType);
    }
    static void ApplyKnockback(Rigidbody2D enemy, Vector2 origin, float force, KnockbackType type)
    {

        Vector3 direction = type switch
        {
            KnockbackType.Directional => origin,
            KnockbackType.Radial => (enemy.position - origin).normalized,
            _ => Vector3.zero
        };
        enemy.linearVelocity = force * direction.normalized;
    }
}