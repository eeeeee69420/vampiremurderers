using UnityEditor;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class GameMenu : MenuManager
{
    public RectTransform itemMenu;
    public Image itemIcon;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public Button takeButton;
    public Button passButton;

    public RectTransform levelUpMenu;
    public void Start()
    {
        Exit(loadingScreen, UIDirection.Top);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }
    public void ItemPickup(PlayerController player, WeaponData weapon = null, PassiveData passive = null)
    {
        if (weapon == null && passive == null) { return; }
        Pause();
        OpenFromTop(itemMenu);
        if (weapon != null)
        {
            itemIcon.sprite = weapon.icon;
            itemName.text = weapon.displayName;
            itemDescription.text = GenerateWeaponDescription(player, weapon);
            takeButton.onClick.RemoveAllListeners();
            takeButton.onClick.AddListener();
        }
    }
    public string GenerateWeaponDescription(PlayerController player, WeaponData weapon)
    {
        Weapon currentWeapon = player.weapons.Find(w => w.weaponData == weapon);

        if (currentWeapon == null)
        {
            return weapon.description;
        }
        if (currentWeapon.level >= weapon.LevelStats.Count)
        {
            return "Max Level";
        }
        string description = "";
        var nextLevelData = weapon.LevelStats[currentWeapon.level];

        foreach (var upgrade in nextLevelData.statIncreases)
        {
            if (description != "") description += "\n";

            description += FormatStatLine(upgrade.stat, upgrade.amount);
        }

        return description;
    }

    private string FormatStatLine(StatType stat, float amount)
    {
        string trend = amount >= 0 ? "Increases" : "Decreases";
        float absoluteAmount = Mathf.Abs(amount);

        bool isPercentage = IsStatPercentage(stat);
        string valueSuffix = isPercentage ? $"{(absoluteAmount * 100)}%" : $"{absoluteAmount}";

        return $"{trend} {InsertSpaces(stat.ToString())} by {valueSuffix}";
    }

    private bool IsStatPercentage(StatType stat)
    {
        switch (stat)
        {
            case StatType.HpMax:
            case StatType.Armor:
            case StatType.Amount:
            case StatType.Revives:
            case StatType.Pierce:
                return false;
            default:
                return true;
        }
    }
    private string InsertSpaces(string text)
    {
        return System.Text.RegularExpressions.Regex.Replace(text, "([a-z])([A-Z])", "$1 $2");
    }
}