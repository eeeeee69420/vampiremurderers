using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public Rigidbody2D enemyBody;
    public SpriteRenderer enemySprite;
    public EnemyAnimator enemyAnimator;
    public GameController controller;

    public float speed;
    public float closestDistance;
    public int playerTarget;
    public Vector2 direction;
    public Vector2 targetPosition;
    public bool touchingPlayer;

    public float attackCooldown;
    public float remainingCooldown;
    public float attackAnimationDuration;
    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        enemySprite = GetComponentInChildren<SpriteRenderer>();
        enemyAnimator = GetComponent<EnemyAnimator>();
        controller = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void FixedUpdate()
    {
        if (remainingCooldown > 0)
            remainingCooldown -= Time.fixedDeltaTime;
        else if (remainingCooldown < 0)
            remainingCooldown = 0;
        Track();
        Move();
        TryAttack();
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
        remainingCooldown = attackCooldown;
        yield return new WaitForSeconds(attackAnimationDuration);
        if (touchingPlayer)
        {
            GameController.Instance.hitScreenAnim();
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
            touchingPlayer = true;
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            touchingPlayer = false;
        }
    }
}
