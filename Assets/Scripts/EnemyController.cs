using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
public class EnemyController : MonoBehaviour
{
    public float speed;
    public Rigidbody2D enemyBody;
    public SpriteRenderer enemySprite;
    public EnemyAnimator enemyAnimator;
    public GameController controller;
    public float closestDistance;
    public float preferredDistance;
    public float preferredDistanceRange;
    public int playerTarget;
    public Vector2 direction;
    public Vector2 targetPosition;
    public bool touchingPlayer;



    // Start is called before the first frame update
    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        enemySprite = GetComponentInChildren<SpriteRenderer>();
        enemyAnimator = GetComponent<EnemyAnimator>();
        controller = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void FixedUpdate()
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
        if (direction.x < 0)
            enemySprite.flipX = true;
        else if (direction.x > 0)
            enemySprite.flipX = false;
        if (closestDistance < preferredDistance - preferredDistanceRange)
            direction *= -1f;
        else if (closestDistance > preferredDistance + preferredDistanceRange) { }
        else
            direction = Vector2.zero;
        enemyBody.MovePosition(enemyBody.position + speed * Time.fixedDeltaTime * direction);

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
