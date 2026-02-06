using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class CharacterSelectionManager : MonoBehaviour
{
    [Header("Setup")]
    public Transform buttonParent;
    public GameObject buttonPrefab;
    public List<CharacterData> characters;

    [Header("Display UI")]
    public RectTransform startButton;
    public RectTransform descriptionPanel;
    private bool isPanelVisible = false;
    public float transitionDuration;
    public Ease easeOut = Ease.OutQuint;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;
    public Image largePreviewImage;
    public TextMeshProUGUI abilityName;
    public TextMeshProUGUI abilityDescription;
    public Image abilityIcon;
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI weaponDescription;
    public Image weaponIcon;

    [Header("Stats UI")]
    public RectTransform statMenu;
    public TextMeshProUGUI hpMaxText;
    public TextMeshProUGUI hpRegenText;
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI moveSpeedText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI cooldownText;
    public TextMeshProUGUI abilityCooldownText;
    public TextMeshProUGUI areaText;
    public TextMeshProUGUI durationText;
    public TextMeshProUGUI projectileSpeedText;
    public TextMeshProUGUI amountText;
    public TextMeshProUGUI growthText;
    public TextMeshProUGUI revivesText;
    public TextMeshProUGUI greedText;
    public TextMeshProUGUI luckText;
    public TextMeshProUGUI criticalChanceText;
    public TextMeshProUGUI criticalDamageText;
    public TextMeshProUGUI pierceText;
    public TextMeshProUGUI lifestealText;

    void Start()
    {
        foreach (CharacterData character in characters)
        {
            GameObject newBtn = Instantiate(buttonPrefab, buttonParent);
            newBtn.GetComponent<CharacterButton>().Setup(character, this);
        }
    }

    public void DisplayCharacter(CharacterData data)
    {
        nameText.text = data.characterName;
        descText.text = data.description;
        largePreviewImage.sprite = data.icon;
        abilityName.text = data.abilityData.displayName;
        abilityDescription.text = data.abilityData.description;
        abilityIcon.sprite = data.abilityData.icon;
        weaponName.text = data.weaponData.displayName;
        weaponDescription.text = data.weaponData.description;
        weaponIcon.sprite = data.weaponData.icon;

        var stats = data.stats;

        // Survival
        UpdateStatRow(hpMaxText, stats.hpmax, 100f, true);
        UpdateStatRow(hpRegenText, stats.hpregen, 0f, true);
        UpdateStatRow(armorText, stats.armor, 0, true);

        // Movement & Utility
        UpdateStatRow(moveSpeedText, stats.moveSpeed, 1.5f, true);
        UpdateStatRow(luckText, stats.luck, 0f, true, "P0");
        UpdateStatRow(growthText, stats.growth, 0f, true, "P0");
        UpdateStatRow(greedText, stats.greed, 0f, true, "P0");
        UpdateStatRow(revivesText, stats.revives, 0f, true, "N0");

        // Offense
        UpdateStatRow(damageText, stats.damage, 0f, true, "P0");
        UpdateStatRow(areaText, stats.area, 0f, true, "P0");
        UpdateStatRow(projectileSpeedText, stats.projectileSpeed, 0f, true, "P0");
        UpdateStatRow(durationText, stats.duration, 0f, true, "P0");
        UpdateStatRow(amountText, stats.amount, 0, true, "N0");
        UpdateStatRow(pierceText, stats.pierce, 0, true, "N0");

        // Criticals
        UpdateStatRow(criticalChanceText, stats.criticalChance, 0.15f, true, "P0");
        UpdateStatRow(criticalDamageText, stats.criticalDamage, 1.5f, true, "F1", "x");
        UpdateStatRow(lifestealText, stats.lifesteal, 0f, true, "P0");

        // Cooldowns (Lower is better!)
        UpdateStatRow(cooldownText, stats.cooldown, 0f, true, "P0");
        UpdateStatRow(abilityCooldownText, stats.abilityCooldown, 0f, true, "P0");

        if (!isPanelVisible)
        {
            descriptionPanel.DOAnchorPos(new Vector2(0, 0), transitionDuration).SetEase(easeOut);
            statMenu.DOAnchorPos(new Vector2(64, 0), transitionDuration).SetEase(easeOut);

        }
    }
    public void OnMenuClose()
    {
        descriptionPanel.DOAnchorPos(new Vector2(480, 0), transitionDuration).SetEase(easeOut).SetDelay(1);
        statMenu.DOAnchorPos(new Vector2(0, 0), transitionDuration).SetEase(easeOut).SetDelay(1);
    }
    private void UpdateStatRow(TextMeshProUGUI text, float current, float baseVal, bool higherIsBetter, string format = "F1", string suffix = "")
    {
        text.text = current.ToString(format) + suffix;

        if (current == baseVal)
        {
            text.color = Color.white;
        }
        else if (current > baseVal)
        {
            text.color = higherIsBetter ? Color.green : Color.red;
        }
        else
        {
            text.color = higherIsBetter ? Color.red : Color.green;
        }
    }
}
