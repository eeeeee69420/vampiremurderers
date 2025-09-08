using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float cooldown;
    public float remainingCooldown;
    public float range;
    public WeaponStats baseStats;
    public WeaponStats buffStats;
    public Collider2D target;
    public Collider2D[] targets;
    public PlayerController playerController;
    public LayerMask enemyLayer;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void FixedUpdate()
    {
        remainingCooldown -= Time.fixedDeltaTime;
        if (remainingCooldown <= 0 && target != null)
            ActivateWeapon();
    }
    protected virtual void ActivateWeapon()
    {
        FindTarget();
        remainingCooldown = cooldown / (1 + buffStats.cooldown);
    }
    protected virtual void FindTarget()
    {

    }
    protected virtual void LevelStats(int level)
    {

    }
}
