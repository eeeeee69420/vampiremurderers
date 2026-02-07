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

        }
    }
    public string GenerateWeaponDescription(PlayerController player, WeaponData weapon)
    {
        string description = "";
        bool found = false;
        foreach (var playerWeapon in player.weapons)
        {
            if (playerWeapon.weaponData == weapon)
            {
                found = true;
                if (playerWeapon.level >= weapon.LevelStats.Count)
                {
                    description = "Max Level";
                }
                else
                {
                    foreach (var statIncrease in weapon.LevelStats[playerWeapon.level].statIncreases)
                    {
                        description += "";
                    }
                }
            }
        }
        if (!found)
        {
            description = weapon.description;
        }
        return description;
    }
}
