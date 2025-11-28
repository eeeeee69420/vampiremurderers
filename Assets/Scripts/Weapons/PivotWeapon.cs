using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PivotWeapon : ProjectileWeapon
{
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
            StartCoroutine(ActivateWeapon());
        }
        Rotate();
    }
    protected override IEnumerator ActivateWeapon(GameObject hitEnemy = null)
    {
        remainingCooldown = stats.cooldown;
        switch (weaponData.weaponBehavior)
        {
            case WeaponBehavior.Shield:
                {
                    GameObject proj = ProjectileManager.Instance.InstantiateProjectile(
                        weaponData.projectile,
                        pivot.transform.position + pivot.transform.up * weaponData.radius,
                        pivot.transform.rotation,
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
                        yield return new WaitForSeconds(weaponData.amountDelay);
                    }
                    break;
                }
        }
    }
    public void Rotate()
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
    public void RestartDuration()
    {
        if (remainingCooldown <= stats.cooldown)
            StartCoroutine(ActivateWeapon());
        foreach (var projectile in spawnedObjects)
            projectile.GetComponent<ProjectileController>().stats.duration = stats.duration;
    }
}
