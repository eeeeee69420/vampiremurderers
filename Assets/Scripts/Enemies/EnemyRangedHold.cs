using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyRangedHold : EnemyBase
{
    public float range;
    public float preferredDistance;
    public float preferredDistanceRange = .5f;
    public override void Start()
    {
        base.Start();
        range = enemyData.stats.projectileSpeed * enemyData.stats.duration;
        preferredDistance = range / 2;
    }
    public override Vector2 Track()
    {
        closestDistance = Mathf.Infinity;
        for (int i = 0; i < GameController.Instance.Players.Count; i++)
        {
            float dist = Vector2.Distance(body.position, GameController.Instance.Players[i].GetComponent<PlayerController>().body.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                playerTarget = i;
            }
        }
        targetPosition = GameController.Instance.Players[playerTarget].GetComponent<PlayerController>().body.position;
        Vector2 direction = (targetPosition - body.position).normalized;
        if (closestDistance < preferredDistance - preferredDistanceRange)
            direction *= -1f;
        else if (closestDistance > preferredDistance + preferredDistanceRange) { }
        else
            direction = Vector2.zero;
        return direction;
    }
}
