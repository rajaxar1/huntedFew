using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AiState
{
    //player.cs
    [HideInInspector] public bool jump = true;
    [HideInInspector] public bool facingRight = true;

    public Waypoint currentWaypoint;
    public Waypoint[] wayPoints;
    public float speed = 3f;
    public bool inReverse = true;
    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 700;

    public bool isCircular;
    // Always true at the beginning because the moving object will always move towards the first waypoint

    protected int currentIndex = 0;
    protected bool isWaiting = false;
    protected float speedStorage = 0;
    protected GameObject player;

    protected Rigidbody2D rb;


    public float distance1 = 0;
    protected  Mover mover;



    public abstract void move();

    protected abstract void MoveTowardsWaypoint();

    protected abstract void NextWaypoint();

    protected void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = mover.transform.localScale;
        theScale.x *= -1;
        mover.transform.localScale = theScale;
    }

    protected void Pause()
    {
        isWaiting = !isWaiting;
    }

}
