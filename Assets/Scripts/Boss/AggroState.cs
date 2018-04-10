using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroState : AiState {

    public AggroState(Mover mover)
    {
        this.mover = mover;
        this.player = mover.GetPlayer();
        this.wayPoints = mover.getWaypoints();
        this.rb = mover.GetComponent<Rigidbody2D>();
        rb.gravityScale = 2;
        if (wayPoints.Length > 0)
        {
            currentWaypoint = ClosestWaypoint();
        }
    }

    public override void move()
    {
        // Get the moving objects current position
        Vector3 currentPosition = mover.transform.position;

        // Get the target waypoints position
        Vector3 targetPosition = player.transform.position;

        distance1 = Vector3.Distance(currentPosition, targetPosition);
        // If the moving object isn't that close to the waypoint
        if (Vector3.Distance(currentPosition, targetPosition) < 13f)
        {

            // Get the direction and normalize
            Vector3 directionOfTravel = targetPosition - currentPosition;
            directionOfTravel.Normalize();

            if (targetPosition.y - currentPosition.y > 2 && jump == false)
            {
                Debug.Log("jump");
                rb.AddForce(new Vector2(0f, jumpForce));
                jump = true;
            }

            if (targetPosition.y - currentPosition.y > 2 && jump == true)
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


        }else if (Vector3.Distance(mover.transform.position, player.transform.position) >= 13f)
        {
            mover.SwitchToFleeState();
        }
    }

    protected override void MoveTowardsWaypoint()
    {
        
    }

    protected override void NextWaypoint()
    {
        
    }

}
