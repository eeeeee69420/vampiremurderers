using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : CharacterController
{
    [HideInInspector] public float inputX;
    [HideInInspector] public float inputY;

    [HideInInspector] public List<Passive> Passives;
    public List<Image> WeaponIcons;
    public List<Image> PassiveIcons;

    [HideInInspector] public float xp;
    [HideInInspector] public float maxXp;
    public float xpScaling;
    [HideInInspector] public int level;

    public new void Start()
    {
        base.Start();
        stats = characterData.stats.Clone();
        hp = stats.hpmax;
        AddWeapon(characterData.weaponData);
        UpdateWeapons();
        UpdatePassives();
    }
    public override Vector2 Track()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
        Vector2 direction = new(inputX, inputY);
        if (direction.magnitude > 1)
            direction = direction.normalized;
        if (direction.magnitude > 0.2)
            characterAnimator.animator.SetBool("isMoving", true);
        else
            characterAnimator.animator.SetBool("isMoving", false);
        return direction;
    }
    public void AddWeapon(WeaponData weaponData)
    {
        bool isNewWeapon = true;
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].weaponData == weaponData)
            {
                isNewWeapon = false;
                if (weapons[i].level < weaponData.LevelStats.Count)
                {
                    weapons[i].LevelWeapon();
                }
            }
        }
        if (isNewWeapon)
        {
            Type behaviorType = WeaponBehaviors.behaviorMap[weaponData.weaponBehavior];
            Weapon newWeapon = (Weapon)gameObject.AddComponent(behaviorType);
            weapons.Add(newWeapon);
            newWeapon.weaponData = weaponData;
            newWeapon.Initiate();
        }
        UpdateWeapons();
    }
    public void AddPassive(PassiveData passiveData)
    {
        {
            bool isNewPassive = true;
            for (int i = 0; i < Passives.Count; i++)
            {
                if (Passives[i].data == passiveData && Passives[i].level <= passiveData.maxLevel)
                {
                    isNewPassive = false;
                    LevelPassive(Passives[i]);
                }
            }
            if (isNewPassive)
            {
                Passive newPassive = new()
                {
                    data = passiveData
                };
                Passives.Add(newPassive);
                UpdatePassives();
            }
        }
    }
    public void LevelPassive(Passive passive)
    {
        passive.level++;
        buffs.MergeBuff(passive.data.bonusPerLevel, passive.data.affectedStat);
        RefreshStats();
    }
    public void UpdateWeapons()
    {
        weapons = new List<Weapon>(GetComponents<Weapon>());
        for (int i = 0; i < WeaponIcons.Count; i++)
        {
            if (i < weapons.Count && weapons[i] != null)
            {
                WeaponIcons[i].sprite = weapons[i].weaponData.icon;
                WeaponIcons[i].color = Color.white;
            }
            else
                WeaponIcons[i].color = Color.clear;
        }
    }
    public override void TakeDamage(float damage, ElementType element = ElementType.Typeless)
    {
        GameController.Instance.HitScreenAnim();
        GameController.Instance.UpdateHPBar();
        hp -= (damage - stats.armor);
        if (hp < 0 && stats.revives == 0)
        {
            StartCoroutine(Death());
        }
        GameController.Instance.ShowDamage(damage, element, transform.position);
    }
    public void UpdatePassives()
    {
        stats = characterData.stats.Clone();
        foreach (var passiveIcon in PassiveIcons)
        {
            if (PassiveIcons.IndexOf(passiveIcon) < Passives.Count && passiveIcon != null)
            {
                passiveIcon.sprite = Passives[PassiveIcons.IndexOf(passiveIcon)].data.icon;
                passiveIcon.color = Color.white;
            }
            else
                passiveIcon.color = Color.clear;
        }
    }
    public void PickUpItem(RewardContainer rewards)
    {
        AddXp(rewards.xpAmount);
        if (rewards.weapon != null)
            AddWeapon(rewards.weapon);
        if (rewards.passive != null)
            AddPassive(rewards.passive);
    }
    public void AddXp(float xpAmount)
    {
        xp += xpAmount;
        if (xp > maxXp)
        {
            xp -= maxXp;
            maxXp *= xpScaling + 1;
            level++;
        }
    }
    public override void RefreshStats()
    {
        foreach (var passive in Passives)
            buffs.MergeBuff(passive.level * passive.data.bonusPerLevel, passive.data.affectedStat);
        stats = characterData.stats.ApplyBuffs(buffs);
        foreach (var weapon in weapons)
            weapon.RefreshStats();
    }
    public override void RefreshStatusDisplay(StatusCondition statusCondition, bool beingRemoved)
    {
        int stackCount = statusConditions.Count(s => s.displayName == statusCondition.displayName);
        Transform existingIcon = null;
        foreach (Transform icon in statusEffectGrid.transform)
        {
            if (icon.name.StartsWith(statusCondition.displayName))
            {
                existingIcon = icon;
                UpdateStackText(icon, stackCount);
                break;
            }
        }

        if (beingRemoved)
        {
            if (stackCount == 0 && existingIcon != null)
            {
                Destroy(existingIcon.gameObject);
            }
        }
        else
        {
            if (existingIcon == null)
            {
                GameObject newIcon = Instantiate(statusEffectPrefab, statusEffectGrid);
                newIcon.transform.Find("StatusIcon").GetComponent<Image>().sprite = statusCondition.icon;
                UpdateStackText(newIcon.transform, stackCount);
                newIcon.name = $"{statusCondition.displayName}_0";
            }
        }
    }
    private void UpdateStackText(Transform icon, int stackCount)
    {
        var text = icon.GetComponentInChildren<TextMeshProUGUI>();
        text.text = stackCount > 1 ? stackCount.ToString() : "";
    }
}
