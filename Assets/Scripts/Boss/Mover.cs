using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
	public Transform sightStart, sightEnd, sightEndUp, sightEndDown;
	public RaycastHit2D playerSeen;
	public int playerIsSeen = 0;
    public Waypoint[] waypoints;
    public float rando;

    [HideInInspector] public bool facingRight = true;

    public Transform groundCheck;
    float groundRadius = 0.2f;
    public LayerMask whatIsGround;

    bool grounded = false;

	Vector3 startingPosition;

    private AiState aiState;

    private GameObject player;

    private Rigidbody2D rb;

    public string currentState; //debug purposes
    public int currentWaypoint;

    public bool jump;

    public int sc;

    public void getSC(){
        sc = Boss.setShotCount();
    }

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

    public void SwitchState(bool closeBy){
    	if (sc == 1){
            SwitchToFleeState();
        }
        if (playerIsSeen == 0){
    		SwtichToPatrolState();
    	}
    	else if (playerIsSeen == 1){
    		if (closeBy){
                SwitchToFleeState();
            }
            else{
                SwtichToPatrolState();
            }
    	}
        else if (playerIsSeen > 3){
            SwitchToAggroState();
        }
    	else{
    		if (closeBy){
    			rando = UnityEngine.Random.Range(0.0f, 1.0f);
    			Debug.Log(rando);
    			if (rando <= .9f){
    				SwitchToAggroState();
    			}
    			else{
    				SwitchToFleeState();
    			}
    		}
    		else{
    			SwtichToPatrolState();
    		}
    	}
    }

    public void switchSeen(){
    	playerIsSeen++;
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
		int layerMask = 1 << 11;
		layerMask = ~layerMask;
		playerSeen = Physics2D.Raycast(sightStart.position,sightEnd.position, Mathf.Infinity, layerMask, -Mathf.Infinity, Mathf.Infinity);
		if (playerSeen.collider == null) Debug.Log("Seen");
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
