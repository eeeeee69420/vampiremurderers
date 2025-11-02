using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D playerBody;
    [HideInInspector] public SpriteRenderer playerSprite;
    [HideInInspector] public PlayerAnimator playerAnimator;
    [HideInInspector] public float inputX;
    [HideInInspector] public float inputY;
    [HideInInspector] public Vector2 inputDirection = new();

    public CharacterData characterData;
    [HideInInspector] public CharacterStats stats;
    [HideInInspector] public CharacterStats buffs;
    [HideInInspector] public float hp;

    [HideInInspector] public List<Weapon> Weapons;
    [HideInInspector] public List<Passive> Passives;
    public List<Image> WeaponIcons;
    public List<Image> PassiveIcons;

    [HideInInspector] public float xp;
    [HideInInspector] public float maxXp;
    public float xpScaling;
    [HideInInspector] public int level;

    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        playerAnimator = GetComponent<PlayerAnimator>();
        stats = characterData.stats.Clone();
        hp = stats.hpmax;
        AddWeapon(characterData.weaponData);
        UpdateWeapons();
        UpdatePassives();
    }

    void FixedUpdate()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
        inputDirection = new Vector2(inputX, inputY);
        if (inputDirection.magnitude > 1)
            inputDirection = inputDirection.normalized;
        if (inputDirection.x < 0)
            playerSprite.flipX = true;
        else if (inputDirection.x > 0)
            playerSprite.flipX = false;
        playerBody.MovePosition(playerBody.position + stats.moveSpeed * Time.fixedDeltaTime * inputDirection);
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
    public void TakeDamage(float damage)
    {
        GameController.Instance.HitScreenAnim();
        GameController.Instance.UpdateHPBar();
        hp -= (damage - stats.armor);
    }
    public void LifeSteal()
    {
        int lifesteal = UnityEngine.Random.Range(1, 100);
        if (lifesteal <= stats.lifesteal)
        {
            hp += 1;
        }
    }
    public void UpdatePassives()
    {
        stats = characterData.stats.Clone();
        for (int i = 0; i < PassiveIcons.Count; i++)
        {
            if (i < Passives.Count && Passives[i] != null)
            {
                PassiveIcons[i].sprite = Passives[i].data.icon;
                PassiveIcons[i].color = Color.white;
            }
            else
                PassiveIcons[i].color = Color.clear;
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
    public void RefreshStats()
    {
        stats = characterData.stats.Clone();
        stats.ApplyBuffs(buffs);
    }
}
