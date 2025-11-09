using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedHold : EnemyBase
{
    public float range;
    public float preferredDistance;
    public float preferredDistanceRange = .5f;
    public new void Start()
    {
        base.Start();
    }
    protected override void Move()
    {
        if (closestDistance < preferredDistance - preferredDistanceRange)
            direction *= -1f;
        else if (closestDistance > preferredDistance + preferredDistanceRange) { }
        else
            direction = Vector2.zero;
        if (direction.x < 0)
            sprite.flipX = true;
        else if (direction.x > 0)
            sprite.flipX = false;
        body.MovePosition(body.position + enemyData.stats.moveSpeed * Time.fixedDeltaTime * direction);
    }

    public override void Initialize()
    {
        range = enemyData.stats.projectileSpeed * enemyData.stats.duration;
        preferredDistance = range / 2;
    }
}
