using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D body;
    [HideInInspector] public SpriteRenderer sprite;
    [HideInInspector] public CharacterAnimator characterAnimator;

    [HideInInspector] public float hp;
    [HideInInspector] public CharacterStats stats = new();
    [HideInInspector] public CharacterStats buffs = new();
    [HideInInspector] public List<Weapon> Weapons;

    [HideInInspector] public Vector2 direction = new();

    [HideInInspector] public List<StatusStates> StatusStates;
    [HideInInspector] public bool dead;

    public void Start()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        characterAnimator = GetComponent<CharacterAnimator>();
        RefreshStats();
    }
    protected virtual void FixedUpdate()
    {
        if (!dead)
        Move();
    }
    protected virtual Vector2 Track()
    {
        return new();
    }
    protected virtual void Move()
    {
        Vector2 direction = Track();
        if (direction.x < 0)
            sprite.flipX = true;
        else if (direction.x > 0)
            sprite.flipX = false;
        body.MovePosition(body.position + stats.moveSpeed * Time.fixedDeltaTime * direction);
    }
    public void LifeSteal()
    {
        int lifesteal = UnityEngine.Random.Range(1, 100);
        if (lifesteal <= stats.lifesteal)
        {
            hp += 1;
        }
    }
    public virtual void TakeDamage(float damage)
    {
        hp -= (damage - stats.armor);
        if (hp < 0)
        {
            dead = true;
            hp = 0;
            characterAnimator.PlayAnimation("Death");
        }
    }
    public virtual void RefreshStats()
    {
    }
}
