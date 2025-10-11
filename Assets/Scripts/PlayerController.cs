using System.Collections;
using System.Collections.Generic;
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
    [HideInInspector] public Vector2 inputDirection = new Vector2();

    public CharacterData characterData;
    [HideInInspector] public PlayerStats stats;

    [HideInInspector] public List<Weapon> Weapons;
    [HideInInspector] public List<Passive> Passives;
    public List<Image> WeaponIcons;
    public List<Image> PassiveIcons;

    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        playerAnimator = GetComponent<PlayerAnimator>();
        stats.hp = stats.hpmax;
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
        playerBody.MovePosition(playerBody.position + inputDirection * stats.moveSpeed * Time.fixedDeltaTime);
    }
    public void AddWeapon(WeaponData weaponData)
    {
        Weapon newWeapon = WeaponBehaviors.AddBehaviorTo(gameObject, weaponData.weaponBehavior);
        if (newWeapon != null)
        {
            Weapons.Add(newWeapon);
            Debug.Log($"{weaponData.weaponBehavior} added to player!");
        }
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
        stats.hp -= (damage - stats.armor);
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
                float totalBonus = Passives[i].data.bonusPerLevel * Passives[i].level;
                switch (Passives[i].data.affectedStat)
                {
                    case StatType.HpMax: stats.hpmax += totalBonus; break;
                    case StatType.HpRegen: stats.hpregen += totalBonus; break;
                    case StatType.Armor: stats.armor += (int)totalBonus; break;
                    case StatType.MoveSpeed: stats.moveSpeed += totalBonus; break;
                    case StatType.Damage: stats.damage += totalBonus; break;
                    case StatType.Cooldown: stats.cooldown -= totalBonus; break;
                    case StatType.Area: stats.area += totalBonus; break;
                    case StatType.Duration: stats.duration += totalBonus; break;
                    case StatType.ProjectileSpeed: stats.projectileSpeed += totalBonus; break;
                    case StatType.Amount: stats.amount += (int)totalBonus; break;
                    case StatType.Growth: stats.growth += totalBonus; break;
                    case StatType.Revives: stats.revives += (int)totalBonus; break;
                    case StatType.Greed: stats.greed += totalBonus; break;
                    case StatType.Luck: stats.luck += totalBonus; break;
                    case StatType.CriticalChance: stats.criticalChance += totalBonus; break;
                    case StatType.CriticalDamage: stats.criticalDamage += totalBonus; break;
                    case StatType.Pierce: stats.pierce += (int)totalBonus; break;
                }
            }
            else
                PassiveIcons[i].color = Color.clear;
        }
    }
}
