using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AiState
{
    //player.cs
    public bool jump = true;

    public Waypoint currentWaypoint;
    public Waypoint[] wayPoints;
    public float speed = 3f;
    public bool inReverse = true;

    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 700;

    public bool isCircular;
    // Always true at the beginning because the moving object will always move towards the first waypoint

    public int currentIndex = 0;
    protected bool isWaiting = false;
    protected float speedStorage = 0;
    protected GameObject player;

    protected Rigidbody2D rb;


    public float distance1 = 0;
    protected  Mover mover;



    public abstract void move();

    protected abstract void MoveTowardsWaypoint();

    protected abstract void NextWaypoint();

    protected Waypoint ClosestWaypoint()
    {
        Waypoint waypoint = null ;
        if (wayPoints.Length > 0) waypoint = wayPoints[0];

        for(int i = 0; i < wayPoints.Length; ++i)
        {
            if (Vector3.Distance(mover.transform.position, wayPoints[i].transform.position) <
                Vector3.Distance(mover.transform.position, waypoint.transform.position))
                waypoint = wayPoints[i];
        }

        return waypoint;
    }

    

    protected void Pause()
    {
        isWaiting = !isWaiting;
    }

}
