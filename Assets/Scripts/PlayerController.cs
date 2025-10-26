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
    [HideInInspector] public int level;
    [HideInInspector] public float hp;

    [HideInInspector] public List<Weapon> Weapons;
    [HideInInspector] public List<Passive> Passives;
    public List<Image> WeaponIcons;
    public List<Image> PassiveIcons;

    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        playerAnimator = GetComponent<PlayerAnimator>();
        stats = characterData.stats.Clone();
        hp = stats.hpmax;
        AddWeapon(characterData.weaponData);
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
        Type behaviorType = WeaponBehaviors.behaviorMap[weaponData.weaponBehavior];
        Weapon newWeapon = (Weapon)gameObject.AddComponent(behaviorType);
        Weapons.Add(newWeapon);
        newWeapon.weaponData = weaponData;
        newWeapon.Initiate();
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
}
