using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
	
	public Waypoint[] wayPoints;
	public float speed = 3f;
	public bool isCircular;
	// Always true at the beginning because the moving object will always move towards the first waypoint
	public bool inReverse = true;

	public Waypoint currentWaypoint;
	private int currentIndex   = 0;
	private bool isWaiting     = false;
	private float speedStorage = 0;
	
	//player.cs
	[HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool jump = true;

    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 700;

	public float distance1 = 0;

    public Transform groundCheck;
    float groundRadius = 0.2f;
    public LayerMask whatIsGround;

    bool grounded = false;
	private Rigidbody2D rb;

	int coins = 0;
	Vector3 startingPosition;

	

	/**
	 * Initialisation
	 * 
	 */
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		if(wayPoints.Length > 0) {
			currentWaypoint = wayPoints[0];
		}
	}
	
	

	/**
	 * Update is called once per frame
	 * 
	 */
	void FixedUpdate()
	{
		if(currentWaypoint != null && !isWaiting) {
			MoveTowardsWaypoint();
		}
	}



	/**
	 * Pause the mover
	 * 
	 */
	void Pause()
	{
		isWaiting = !isWaiting;
	}


	
	/**
	 * Move the object towards the selected waypoint
	 * 
	 */
	private void MoveTowardsWaypoint()
	{
		// Get the moving objects current position
		Vector3 currentPosition = this.transform.position;
		
		// Get the target waypoints position
		Vector3 targetPosition = currentWaypoint.transform.position;

		distance1 = Vector3.Distance(currentPosition, targetPosition);
		// If the moving object isn't that close to the waypoint
		if(Vector3.Distance(currentPosition, targetPosition) > 3f) {

			// Get the direction and normalize
			Vector3 directionOfTravel = targetPosition - currentPosition;
			directionOfTravel.Normalize();

			if(targetPosition.y - currentPosition.y > 2 && jump == false){
				 rb.AddForce(new Vector2(0f, jumpForce));
				 jump = true;
			}

			if(targetPosition.y - currentPosition.y < 1 && jump == true){
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
			if(directionOfTravel.x * rb.velocity.x < maxSpeed)
            	rb.AddForce(Vector2.right * directionOfTravel.x * moveForce);
        
        	if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            	rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);

        	if (directionOfTravel.x > 0 && !facingRight)
            	Flip();
        	else if (directionOfTravel.x < 0 && facingRight)
            	Flip();

		} else {
			
			// If the waypoint has a pause amount then wait a bit
			if(currentWaypoint.waitSeconds > 0) {
				Pause();
				Invoke("Pause", currentWaypoint.waitSeconds);
			}

			// If the current waypoint has a speed change then change to it
			if(currentWaypoint.speedOut > 0) {
				speedStorage = speed;
				speed = currentWaypoint.speedOut;
			} else if(speedStorage != 0) {
				speed = speedStorage;
				speedStorage = 0;
			}

			NextWaypoint();
		}
	}

	void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }


	/**
	 * Work out what the next waypoint is going to be
	 * 
	 */
	private void NextWaypoint()
	{


		
		
		if(isCircular) {
			
			if(!inReverse) {
				currentIndex = (currentIndex+1 >= wayPoints.Length) ? 0 : currentIndex+1;
			} else {
				currentIndex = (currentIndex == 0) ? wayPoints.Length-1 : currentIndex-1;
			}

		} else {
			
			// If at the start or the end then reverse
			if((!inReverse && currentIndex+1 >= wayPoints.Length) || (inReverse && currentIndex == 0)) {
				inReverse = !inReverse;
			}
			currentIndex = (!inReverse) ? currentIndex+1 : currentIndex-1;

		}

		currentWaypoint = wayPoints[currentIndex];
	}
}
