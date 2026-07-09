using DG.Tweening;
using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MenuManager
{
    public RectTransform itemMenu;
    public Image itemIcon;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public Button takeButton;
    public Button passButton;

    public RectTransform levelUpMenu;
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
        if (weapon == null && passive == null) return;

        Pause();
        Enter(itemMenu, UIDirection.Top);

        bool isMaxed = false;
        bool isFull = false;

        if (weapon != null)
        {
            itemIcon.sprite = weapon.icon;
            itemName.text = weapon.displayName;
            itemDescription.text = GenerateWeaponDescription(player, weapon);

            Weapon current = player.weapons.Find(w => w.weaponData == weapon);
            isMaxed = (itemDescription.text == "Max Level");
            isFull = (current == null && player.weapons.Count >= 6);
        }
        else if (passive != null)
        {
            itemIcon.sprite = passive.icon;
            itemName.text = passive.displayName;
            itemDescription.text = GeneratePassiveDescription(player, passive);

            Passive current = player.Passives.Find(p => p.passiveData == passive);
            isMaxed = (itemDescription.text == "Max Level");
            isFull = (current == null && player.Passives.Count >= 6);
        }

        takeButton.interactable = !isMaxed && !isFull;

        takeButton.onClick.RemoveAllListeners();
        takeButton.onClick.AddListener(() => player.PickUpItem(weapon, passive));
        takeButton.onClick.AddListener(() => StartCoroutine(ExitPickupMenu()));
    }
    public IEnumerator ExitPickupMenu()
    {
        Exit(itemMenu, UIDirection.Top);
        yield return new WaitForSecondsRealtime(1f);
        Resume();
    }
    public string GeneratePassiveDescription(PlayerController player, PassiveData passive)
    {
        Passive currentPassive = player.Passives.Find(p => p.passiveData == passive);
        if (currentPassive != null && currentPassive.level >= passive.maxLevel)
        {
            return "Max Level";
        }
        return FormatStatLine(passive.affectedStat, passive.bonusPerLevel);
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

        return $" - {trend} {InsertSpaces(stat.ToString())} by {valueSuffix}.";
    }

    private bool IsStatPercentage(StatType stat)
    {
        return stat switch
        {
            StatType.HpMax or StatType.Armor or StatType.Amount or StatType.Revives or StatType.Pierce => false,
            _ => true,
        };
    }
    private string InsertSpaces(string text)
    {
        return System.Text.RegularExpressions.Regex.Replace(text, "([a-z])([A-Z])", "$1 $2");
    }
}