using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
	public Transform sightStart, sightEnd, sightEndUp, sightEndDown;
	public bool playerSeen = false;

    public Waypoint[] waypoints;

    public Transform groundCheck;
    float groundRadius = 0.2f;
    public LayerMask whatIsGround;

    bool grounded = false;

	int coins = 0;
	Vector3 startingPosition;

    private AiState aiState;

    private GameObject player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start () {
        aiState = new PatrolState(this);
	}

    public void SwtichToPatrolState()
    {
        aiState = new PatrolState(this);
    }

    public void SwitchToFleeState()
    {
        aiState = new FleeState(this);
    }

    public void SwitchToAggroState()
    {
        aiState = new AggroState(this);
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
		Raycasting();
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
	
}
