using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	float speed = 3f;
	Rigidbody2D rb;
	int coins = 0;
	Vector3 startingPosition;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		startingPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		var input = Input.GetAxis("Horizontal");
		var movement = input*speed;

		rb.velocity = new Vector3(movement, rb.velocity.y, 0);

		if (Input.GetKeyDown(KeyCode.Space)){
			rb.AddForce(new Vector3(0,100,0));
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Coin"){
			coins++;
			Destroy(col.gameObject);
		}
		else if (col.tag == "Water"){
			transform.position = startingPosition;
		}
		else if (col.tag == "Spike"){
			transform.position = startingPosition;
		}
	}
}
