using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager Instance;

    public GameObject universalProjectilePrefab;
    public GameObject universalPivotPrefab;

    public List<GameObject> projectiles;
    public List<GameObject> disabledProjectiles;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public GameObject InstantiateProjectile(ProjectileData projectileData, Vector3 position, Quaternion firingAngle, GameObject target, CharacterController owner, CharacterStats stats, bool isPlayer, List<StatusCondition> statusConditions, ElementType element, Transform parent = null)
    {
        GameObject projectile;
        if (disabledProjectiles.Count == 0)
        {
            projectile = Instantiate(universalProjectilePrefab);
            projectiles.Add(projectile);
        }
        else
        {
            projectile = disabledProjectiles[0];
            disabledProjectiles.RemoveAt(0);
        }
        if (parent != null)
            projectile.transform.SetParent(parent.transform, false);
        else
            projectile.transform.SetParent(null);

            projectile.transform.SetPositionAndRotation(position, firingAngle);
        projectile.transform.localScale = Vector3.one;

        ProjectileController projectileController = projectile.GetComponent<ProjectileController>();
        projectileController.data = projectileData;
        projectileController.target = target;
        projectileController.owner = owner;
        projectileController.stats = stats.Clone();
        projectileController.isPlayer = isPlayer;
        projectileController.statusConditions = statusConditions;
        projectileController.element = element;
        projectileController.Initialize();
        projectile.SetActive(true);
        return projectile;
    }
    public void DisableProjectile(GameObject projectile)
    {
        projectile.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        projectile.transform.localScale = Vector3.one;
        projectile.SetActive(false);
        projectile.GetComponent<ProjectileController>().hitObjects.Clear();
        disabledProjectiles.Add(projectile);
    }
}
