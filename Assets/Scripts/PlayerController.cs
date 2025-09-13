using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public float speed;
    public Rigidbody2D playerBody;
    public SpriteRenderer playerSprite;
    public PlayerAnimator playerAnimator;
    public float inputX;
    public float inputY;
    public Vector2 inputDirection = new Vector2();

    public PlayerStats stats;

    public List<Weapon> Weapons;
    public List<Image> WeaponIcons;
    public List<Image> PassiveIcons;

    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        playerAnimator = GetComponent<PlayerAnimator>();
        stats.hp = stats.hpmax;
        NewWeapon(gameObject.GetComponent<Weapon>());
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
        playerBody.MovePosition(playerBody.position + inputDirection * speed * Time.fixedDeltaTime);
    }
    void NewWeapon(Weapon weapon)
    {
        bool added = false;
        for (int i = 0; i < Weapons.Count; i++)
        {
            if (Weapons[i] == null)
            {
                Weapons[i] = weapon;
                added = true;
                break;
            }
        }
        if (!added)
            Weapons.Add(weapon);
        WeaponIcons[Weapons.IndexOf(weapon)].sprite = weapon.icon;
    }
}
