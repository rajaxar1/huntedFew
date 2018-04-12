using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
	public Transform sightStart, sightEnd, sightEndUp, sightEndDown;
	public bool playerSeen = false;

    public Waypoint[] waypoints;

    [HideInInspector] public bool facingRight = true;

    public Transform groundCheck;
    float groundRadius = 0.2f;
    public LayerMask whatIsGround;

    bool grounded = false;

	int coins = 0;
	Vector3 startingPosition;

    private AiState aiState;

    private GameObject player;

    private Rigidbody2D rb;

    public string currentState; //debug purposes
    public int currentWaypoint;

    public bool jump;

    void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start () {
        SwtichToPatrolState();
	}

    public void SwtichToPatrolState()
    {
        Debug.Log("PatrolState");
        aiState = new PatrolState(this);
        currentState = "PatrolState";
    }

    public void SwitchToFleeState()
    {
        Debug.Log("FleeState");
        aiState = new FleeState(this);
        currentState = "FleeState";
    }

    public void SwitchToAggroState()
    {
        Debug.Log("AggroState");
        aiState = new AggroState(this);
        currentState = "AggroState";
    }

    public GameObject GetPlayer()
    {
        return player;
    }

	/**
	 * Update is called once per frame
	 * 
	 */
	void FixedUpdate()
	{
        aiState.move();
        SetFacingDirection();
		Raycasting();
        currentWaypoint=  aiState.currentIndex;
        jump = aiState.jump;
	}

    public Waypoint[] getWaypoints()
    {
        return waypoints;
    }
	

	void Raycasting(){
		Debug.DrawLine(sightStart.position,sightEnd.position,Color.white);
		Debug.DrawLine(sightStart.position,sightEndUp.position,Color.white);
		Debug.DrawLine(sightStart.position,sightEndDown.position,Color.white);

		playerSeen = Physics2D.Linecast(sightStart.position,sightEnd.position, 1 <<LayerMask.NameToLayer("Player"));
		if (playerSeen) Debug.Log("Seen");
	}

    internal void SetFacingDirection()
    {
        if (rb.velocity.x > 0 && !facingRight)
            Flip();
        else if (rb.velocity.x < 0 && facingRight)
            Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
