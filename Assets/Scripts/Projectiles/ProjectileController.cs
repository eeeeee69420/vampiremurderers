using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using static UnityEngine.EventSystems.EventTrigger;

public class ProjectileController : MonoBehaviour
{
    public Rigidbody2D projectileBody;
    public Animator animator;
    public SpriteRenderer sprite;
    public Transform spriteObject;
    public CircleCollider2D circleCollider;

    public List<GameObject> hitObjects = new();

    public CharacterStats stats;
    public CharacterController owner;
    public GameObject target;
    public List<StatusCondition> statusConditions;
    public ElementType element;
    public bool isPlayer;
    public bool isDespawned;

    public ProjectileData data;
    void Awake()
    {
        projectileBody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
    }
    public void Initialize()
    {
        if (data.usesAnimator)
            animator.runtimeAnimatorController = data.animatorController;
        else
        {
            sprite.sprite = data.staticSprite;
            animator.runtimeAnimatorController = null;
        }
        spriteObject.localPosition = data.spriteOffset;
        spriteObject.localScale = data.spriteScale;
        spriteObject.localRotation = Quaternion.Euler(0f, 0f, data.spriteRotation);

        circleCollider.offset = data.hitboxOffset;
        circleCollider.radius = data.hitboxSize;
        circleCollider.isTrigger = data.isTrigger;
        if (!data.isTrigger)
        {
            circleCollider.excludeLayers = 1 << owner.gameObject.layer;
        }

        transform.localScale *= stats.area;

        isDespawned = false;
    }
    void FixedUpdate()
    {
        Move();
        stats.duration -= Time.fixedDeltaTime;
        if (stats.duration < 0)
            Despawn();
    }
    protected virtual void Move()
    {
        if (data.spriteFacesUp)
            spriteObject.rotation = Quaternion.identity; 
        switch (data.movement)
        {
            case ProjectileMovement.None:
                break;
            case ProjectileMovement.Straight:
                projectileBody.position += (Vector2)(stats.projectileSpeed * Time.fixedDeltaTime * transform.up);
                break;
        }
    }
    protected virtual void Pierce()
    {
        stats.pierce -= 1;
        if (stats.pierce == 0)
            Despawn();
    }
    public virtual void Despawn()
    {
        ProjectileManager.Instance.DisableProjectile(gameObject);
        isDespawned = true;
    }
    protected virtual void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (((isPlayer && collidedObject.layer == 8) || (!isPlayer && collidedObject.layer == 6)) && !hitObjects.Contains(collidedObject))
        {
            IWeaponHit.HitEnemy(collidedObject, stats.damage, owner, element, statusConditions, stats.projectileSpeed, KnockbackType.Directional, transform.up);
            hitObjects.Add(collidedObject);
            Pierce();
        }
    }
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (((isPlayer && collidedObject.layer == 8) || (!isPlayer && collidedObject.layer == 6)) && !hitObjects.Contains(collidedObject))
        {
            IWeaponHit.HitEnemy(collidedObject, stats.damage, owner, element, statusConditions, stats.projectileSpeed, KnockbackType.Radial, transform.position);
            hitObjects.Add(collidedObject);
            StartCoroutine(MarkUnhit(collidedObject));
        }
        else if (collidedObject.layer == 9 && collidedObject.GetComponent<ProjectileController>().isPlayer != isPlayer)
        {
            collidedObject.GetComponent<ProjectileController>().Despawn();
        }
    }


    protected void OnCollisionStay2D(Collision2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.layer == 8)
        {
            IWeaponHit.HitEnemy(collidedObject, stats.damage * Time.deltaTime * 2, owner, element, statusConditions, stats.projectileSpeed, KnockbackType.Radial, owner.transform.position);
        }
    }
    IEnumerator MarkUnhit(GameObject gameObject)
    {
        yield return new WaitForSeconds(.5f);
        hitObjects.Remove(gameObject);
    }
}
