using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class AbilityController : MonoBehaviour
{
    [HideInInspector] public AbilityData abilityData;
    [HideInInspector] public CharacterController controller;
    [HideInInspector] public float cooldown;
    [HideInInspector] public bool durating;

    public Image abilityIcon;
    public Image abilityIconOverlay;
    public TextMeshProUGUI abilityText;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
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
            abilityText.color = Color.black;
            durating = false;
            controller.characterAnimator.animator.SetBool("isUsingAbility", false);
        }
        else cooldown -= Time.deltaTime;
            abilityIconOverlay.fillAmount = 1 - cooldown / abilityData.cooldown;
        abilityText.text = cooldown.ToString("F0");
    }
    protected virtual void ActivateAbility()
    {
        controller.characterAnimator.animator.SetBool("isUsingAbility", true);
        abilityText.color = Color.cyan;
        cooldown = abilityData.cooldown / (1 + controller.stats.cooldown);
        durating = true;
        foreach (var status in abilityData.statusConditions)
        {
            StatusCondition statusClone = status.Clone();
            statusClone.remainingDuration = statusClone.duration * (controller.buffs.duration + 1);
            if (statusClone.delayUsesDuration)
                statusClone.delay *= controller.buffs.duration + 1;
            StartCoroutine(controller.AddStatus(statusClone));
        }
    }
}
