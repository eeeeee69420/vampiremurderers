using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public GameObject WeaponGrid;
    public List<Weapon> Weapons;
    public GameObject PassiveGrid;

    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        playerAnimator = GetComponent<PlayerAnimator>();
        stats.hp = stats.hpmax;
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
}
