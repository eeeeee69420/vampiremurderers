using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectileData", menuName = "Game/Projectile Data")]
public class ProjectileData : ScriptableObject
{
    public Sprite staticSprite;
    public bool usesAnimator;
    public RuntimeAnimatorController animatorController;
    public bool spriteFacesUp;
    public Vector2 spriteOffset; //Try to keep the center of the sprite or the center of the hitbox at 0,0.
    public Vector2 spriteScale; //Always solid numbers (0.25, .5, 1, 1.5, 2, etc).
    public float spriteRotation; //Always face straight up.
    public float spriteRotationSpeed;
    public GameObject VFXPrefab;

    public ColliderType colliderType;
    public Vector2 hitboxSize;
    public Vector2 hitboxOffset;
    public bool isTrigger;

    public ProjectileData deathProjectile;
    public ProjectileMovement movement;
    public float acceleration;
    public float turnSpeed;
    public bool destroyEnemyProjectiles;
}
