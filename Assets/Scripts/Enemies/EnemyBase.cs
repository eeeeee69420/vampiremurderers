using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D enemyBody;
    [HideInInspector] public SpriteRenderer enemySprite;
    [HideInInspector] public EnemyAnimator enemyAnimator;
    [HideInInspector] public GameController controller;

    public float speed;
    [HideInInspector] public float closestDistance;
    [HideInInspector] public int playerTarget;
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public Vector2 targetPosition;
    [HideInInspector] public bool touchingPlayer;

    public float attackCooldown;
    [HideInInspector] public float remainingCooldown;
    public float attackAnimationDuration;
    public EnemyData enemyData;
    [HideInInspector] public bool dead;
    [HideInInspector] public float freezeTimer;

    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        enemySprite = GetComponentInChildren<SpriteRenderer>();
        enemyAnimator = GetComponent<EnemyAnimator>();
        enemyAnimator.animator.runtimeAnimatorController = EnemyBehaviors.behaviorMap[enemyData.behavior].controller;
        controller = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void FixedUpdate()
    {
        if (!dead && freezeTimer == 0)
        {
            if (remainingCooldown > 0)
                remainingCooldown -= Time.fixedDeltaTime;
            else if (remainingCooldown < 0)
                remainingCooldown = 0;
            Track();
            Move();
            TryAttack();
        }
        else if (freezeTimer > 0)
            freezeTimer -= Time.fixedDeltaTime;
        else if (freezeTimer < 0)
            freezeTimer = 0;
    }
    protected virtual void Track()
    {
        closestDistance = Mathf.Infinity;
        for (int i = 0; i < controller.Players.Count; i++)
        {
            float dist = Vector2.Distance(enemyBody.position, controller.Players[i].GetComponent<PlayerController>().playerBody.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                playerTarget = i;
            }
        }
        targetPosition = controller.Players[playerTarget].GetComponent<PlayerController>().playerBody.position;
        direction = (targetPosition - enemyBody.position).normalized;
    }
    protected virtual void Move()
    {
        if (direction.x < 0)
            enemySprite.flipX = true;
        else if (direction.x > 0)
            enemySprite.flipX = false;
        enemyBody.MovePosition(enemyBody.position + speed * Time.fixedDeltaTime * direction);
    }
    protected virtual void TryAttack()
    {

        if (touchingPlayer && remainingCooldown <= 0)
            StartCoroutine(AttackPlayer());

    }
    protected virtual IEnumerator AttackPlayer()
    {
        enemyAnimator.PlayAnimation("Attack");
        remainingCooldown += attackCooldown;
        yield return new WaitForSeconds(attackAnimationDuration);
        if (touchingPlayer)
            GameController.Instance.Players[playerTarget].GetComponent<PlayerController>().TakeDamage(enemyData.stats.damage);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
            touchingPlayer = true;
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
            touchingPlayer = false;
    }
    public void hit()
    {
        if (enemyData.stats.hp < 0)
        {
            dead = true;
            enemyData.stats.hp = 0;
            enemyAnimator.PlayAnimation("Death");
        }
    }
    public void death()
    {
        Destroy(gameObject);
    }
    public virtual void Intialize()
    {

    }
}
