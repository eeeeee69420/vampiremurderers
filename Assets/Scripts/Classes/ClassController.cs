using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class ClassController : MonoBehaviour
{
    [HideInInspector] public AbilityData abilityData;
    [HideInInspector] public PlayerController controller;
    [HideInInspector] public float cooldown;
    [HideInInspector] public bool durating;

    public Image abilityIcon;
    public Image abilityIconOverlay;
    public TextMeshProUGUI abilityText;
    private void Start()
    {
        controller = GetComponent<PlayerController>();
        abilityData = controller.characterData.abilityData;
        abilityIcon.sprite = abilityData.icon;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && cooldown <= 0 && !durating)
            ActivateAbility();
        if (cooldown < 0)
        {
            cooldown = 0;
        }
        if (durating && cooldown <= 0)
        {
            abilityText.color = Color.white;
            durating = false;
            controller.characterAnimator.animator.SetBool("isUsingAbility", false);
            cooldown = abilityData.cooldown / (1 + controller.stats.cooldown);
        }
        else cooldown -= Time.deltaTime;
            abilityIconOverlay.fillAmount = cooldown / abilityData.cooldown;
        abilityText.text = cooldown > 0 ? cooldown.ToString("F0") : "";
    }
    protected virtual void ActivateAbility()
    {
        controller.characterAnimator.animator.SetBool("isUsingAbility", true);
        abilityText.color = Color.cyan;
        cooldown = abilityData.duration * (1 + controller.stats.duration);
        durating = true;
        foreach (var status in abilityData.statusConditions)
        {
            StartCoroutine(controller.AddStatus(status, controller));
        }
        foreach (var weapon in abilityData.tempWeapons)
            controller.AddWeapon(weapon);
    }
    protected virtual void DeactivateAbility()
    {
        var statusesToRemove = controller.statusConditions
            .Where(status => abilityData.statusConditions
                .Any(abilityStatus => abilityStatus.displayName == status.displayName))
            .ToList();
        foreach (var status in statusesToRemove)
        {
            controller.RemoveStatus(status);
        }
    }
}
