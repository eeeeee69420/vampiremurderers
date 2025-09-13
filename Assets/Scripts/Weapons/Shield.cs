using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheild : Weapon
{
    public GameObject shield;
    public GameObject shieldPrefab;
    public float holdDistance;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        RefreshStats();
        shield = Instantiate(shieldPrefab);
    }
    protected override IEnumerator ActivateWeapon()
    {
        shield.transform.localPosition = playerController.transform.localPosition + new Vector3(playerController.inputDirection.x * holdDistance, playerController.inputDirection.y, 0);
        yield return null;
    }
}
