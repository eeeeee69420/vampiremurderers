using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : CharacterController
{
    [HideInInspector] public float inputX;
    [HideInInspector] public float inputY;

    public CharacterData characterData;

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
        for (int i = 0; i < Weapons.Count; i++)
        {
            if (Weapons[i].weaponData == weaponData)
            {
                isNewWeapon = false;
                if (Weapons[i].level < weaponData.LevelStats.Count)
                {
                    Weapons[i].LevelWeapon();
                }
            }
        }
        if (isNewWeapon)
        {
            Type behaviorType = WeaponBehaviors.behaviorMap[weaponData.weaponBehavior];
            Weapon newWeapon = (Weapon)gameObject.AddComponent(behaviorType);
            Weapons.Add(newWeapon);
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
        Weapons = new List<Weapon>(GetComponents<Weapon>());
        for (int i = 0; i < WeaponIcons.Count; i++)
        {
            if (i < Weapons.Count && Weapons[i] != null)
            {
                WeaponIcons[i].sprite = Weapons[i].weaponData.icon;
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
        stats = characterData.stats.Clone();
        stats.ApplyBuffs(buffs);
    }
}
