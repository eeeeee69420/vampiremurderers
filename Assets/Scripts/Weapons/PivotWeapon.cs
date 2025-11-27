using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PivotWeapon : ProjectileWeapon
{
    public bool durating;
    public GameObject pivot;

    public override void Initiate()
    {
        base.Initiate();
        pivot = Instantiate(ProjectileManager.Instance.universalPivotPrefab, transform.position, Quaternion.identity, gameObject.transform);
    }

    public override void FixedUpdate()
    {
        Debug.Log(remainingCooldown);
        remainingCooldown -= Time.deltaTime;
        if (remainingCooldown < 0)
            remainingCooldown = 0;
        if (remainingCooldown <= 0)
        {
            if (durating)
                Deactivate();
            else
                Activate();
        }
        Rotate();
    }
    public void Activate()
    {
        durating = true;
        remainingCooldown = stats.duration;

        switch (weaponData.weaponBehavior)
        {
            case WeaponBehavior.Shield:
                {
                    GameObject proj = ProjectileManager.Instance.InstantiateProjectile(
                        weaponData.projectile,
                        new Vector3(0, weaponData.radius, 0) + transform.position,
                        Quaternion.identity,
                        gameObject,
                        gameObject.GetComponent<CharacterController>(),
                        stats,
                        gameObject.layer == 6,
                        weaponData.statusConditions,
                        weaponData.element,
                        pivot.transform
                        );
                    spawnedObjects.Add(proj);
                    break;
                }
            case WeaponBehavior.OrbittingProjectile:
                {
                    float step = 360f / stats.amount;

                    for (int i = 0; i < stats.amount; i++)
                    {
                        float angle = i * step;
                        float rad = angle * Mathf.Deg2Rad;
                        Vector2 dir = new(Mathf.Cos(rad), Mathf.Sin(rad));
                        Vector3 spawnPosition = transform.position + (Vector3)(dir * weaponData.radius);
                        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

                        GameObject proj = ProjectileManager.Instance.InstantiateProjectile(
                            weaponData.projectile,
                            spawnPosition,
                            rotation,
                            gameObject,
                            gameObject.GetComponent<CharacterController>(),
                            stats,
                            gameObject.layer == 6,
                            weaponData.statusConditions,
                            weaponData.element
                        );
                        spawnedObjects.Add(proj);
                    }
                    break;
                }
        }
    }
    public void Deactivate()
    {
        durating = false;
        remainingCooldown = stats.cooldown;
        foreach (var projectile in spawnedObjects)
        {
            projectile.SetActive(false);
        }
    }
    public void Rotate()
    {
        if (durating)
        {
            switch (weaponData.weaponBehavior)
            {
                case WeaponBehavior.OrbittingProjectile:
                    pivot.transform.Rotate(0f, 0f, weaponData.projectile.turnSpeed * Time.fixedDeltaTime);
                    break;
                case WeaponBehavior.Shield:
                    Vector2 dir = controller.direction;
                    if (dir.magnitude > 0)
                    {
                        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                        pivot.transform.rotation = Quaternion.RotateTowards(pivot.transform.rotation, Quaternion.Euler(0f, 0f, angle - 90f), weaponData.projectile.turnSpeed * Time.fixedDeltaTime);
                    }
                    break;
            }
        }
    }
    public void RestartDuration(float duration)
    {
        Deactivate();

        durating = true;
        remainingCooldown = duration;

        Activate();
    }
}
