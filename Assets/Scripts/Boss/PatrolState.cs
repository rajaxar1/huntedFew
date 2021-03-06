﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : AiState {
    private bool closeBy =false;
    private bool onLadder = false;

    float timer = 0.0f;
    float preventLockTime = 5.0f;

    public PatrolState(Mover mover)
    {
        this.mover = mover;
        this.wayPoints = mover.getWaypoints();
        this.player = mover.GetPlayer();
        this.rb = mover.GetComponent<Rigidbody2D>();
        isCircular = true;
        inReverse = false;
        rb.gravityScale = 2;
        if (wayPoints.Length > 0)
        {
            currentWaypoint = ClosestWaypoint();
        }
    }


    override
    protected void MoveTowardsWaypoint()
    {
        // Get the moving objects current position
        Vector3 currentPosition = mover.transform.position;

        // Get the target waypoints position
        Vector3 targetPosition = currentWaypoint.transform.position;

        if (timer >= preventLockTime) mover.transform.position = targetPosition;

        distance1 = Vector3.Distance(currentPosition, targetPosition);
        // If the moving object isn't that close to the waypoint
        if (Vector3.Distance(currentPosition, targetPosition) > 4f)
        {
            timer += Time.deltaTime;
            //Debug.Log(targetPosition.y - currentPosition.y);
            // Get the direction and normalize
            Vector3 directionOfTravel = targetPosition - currentPosition;
            directionOfTravel.Normalize();

            if (targetPosition.y - currentPosition.y > 1 && jump == false)
            {
                rb.velocity = new Vector3(0,13,0);
                jump = true;
            }

            if (targetPosition.y - currentPosition.y < 1 && jump == true)
            {
                jump = false;
            }

            //rotation based on next waypoint
            rb.MoveRotation(rb.rotation + speed * Time.fixedDeltaTime);

            /*//mover.cs old code 
			//scale the movement on each axis by the directionOfTravel vector components
			this.transform.Translate(
				directionOfTravel.x * speed * Time.deltaTime,
				directionOfTravel.y * speed * Time.deltaTime,
				directionOfTravel.z * speed * Time.deltaTime,
				Space.World
			);*/

            //new code
            if (directionOfTravel.x * rb.velocity.x < maxSpeed)
                rb.AddForce(Vector2.right * directionOfTravel.x * moveForce);

            if (Mathf.Abs(rb.velocity.x) > maxSpeed)
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);

        }
        else
        {

            // If the waypoint has a pause amount then wait a bit
            if (currentWaypoint.waitSeconds > 0)
            {
                Pause();
                mover.Invoke("Pause", currentWaypoint.waitSeconds);
            }

            // If the current waypoint has a speed change then change to it
            if (currentWaypoint.speedOut > 0)
            {
                speedStorage = speed;
                speed = currentWaypoint.speedOut;
            }
            else if (speedStorage != 0)
            {
                speed = speedStorage;
                speedStorage = 0;
            }

            NextWaypoint();
        }

    }

    override
    protected void NextWaypoint()
    {
        timer = 0;

        if (isCircular)
        {

            if (!inReverse)
            {
                currentIndex = (currentIndex + 1 >= wayPoints.Length) ? 0 : currentIndex + 1;
            }
            else
            {
                currentIndex = (currentIndex == 0) ? wayPoints.Length - 1 : currentIndex - 1;
            }

        }
        else
        {

            // If at the start or the end then reverse
            if ((!inReverse && currentIndex + 1 >= wayPoints.Length) || (inReverse && currentIndex == 0))
            {
                inReverse = !inReverse;
            }
            currentIndex = (!inReverse) ? currentIndex + 1 : currentIndex - 1;

        }

        currentWaypoint = wayPoints[currentIndex];
    }

    public override void move()
    {
        closeBy = (Vector3.Distance(mover.transform.position, player.transform.position)) < 10f;
        if (closeBy)
        {
            mover.switchSeen();            
            mover.SwitchState(true);
        }
        if (currentWaypoint != null && !isWaiting)
        {
            MoveTowardsWaypoint();
        }
    }
}
