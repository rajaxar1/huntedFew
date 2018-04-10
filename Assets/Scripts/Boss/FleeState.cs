using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : AiState {

    public FleeState(Mover mover)
    {
        this.mover = mover;
        this.wayPoints = mover.getWaypoints();
        this.rb = mover.GetComponent<Rigidbody2D>();
        rb.gravityScale = 2;
        if (wayPoints.Length > 0)
        {
            currentWaypoint = wayPoints[0];
        }
    }

    public override void move()
    {
        throw new System.NotImplementedException();
    }

    protected override void MoveTowardsWaypoint()
    {
        throw new System.NotImplementedException();
    }

    protected override void NextWaypoint()
    {
        throw new System.NotImplementedException();
    }

}
