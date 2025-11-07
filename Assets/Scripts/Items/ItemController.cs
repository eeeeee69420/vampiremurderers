using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public RewardContainer rewardContainer;
    [HideInInspector] public Rigidbody2D body;
    public bool attracted;
    public float speed;
    public float acceleration;
    [HideInInspector] public Vector2 direction;
    [HideInInspector] float dist;



    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if (attracted)
        {
            FindTarget();
            Move();
        }
    }
    private void FindTarget()
    {
        int playerTarget = 0;
        float closestDistance = Mathf.Infinity;
        for (int i = 0; i < GameController.Instance.Players.Count; i++)
        {
            dist = Vector2.Distance(body.position, GameController.Instance.Players[i].GetComponent<PlayerController>().playerBody.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                playerTarget = i;
            }
        }
        Vector2 targetPosition = GameController.Instance.Players[playerTarget].GetComponent<PlayerController>().playerBody.position;
        direction = (targetPosition - body.position).normalized;
    }
    private void Move()
    {
        speed += acceleration * Time.fixedDeltaTime;
        body.MovePosition(body.position + speed * Time.fixedDeltaTime * direction);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 && !attracted)
        {
            attracted = true;
            speed *= -1;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 && attracted && dist <= 0.2f)
        {
            collision.GetComponent<PlayerController>().PickUpItem(rewardContainer);
            Destroy(gameObject);
        }
    }
}
