using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheild : Weapon
{
    public GameObject shield;
    public GameObject shieldPrefab;
    public float holdDistance;
    public float angle;
    public float rotationSpeed;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        RefreshStats();
        shield = Instantiate(shieldPrefab, playerController.transform);
    }
    protected override IEnumerator ActivateWeapon()
    {
        Vector2 dir = playerController.inputDirection;
        if (dir.magnitude > 0)
            angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        shield.transform.rotation = Quaternion.RotateTowards(shield.transform.rotation, Quaternion.Euler(0f, 0f, angle - 90f), rotationSpeed * Time.fixedDeltaTime);
        yield return null;
    }
}
