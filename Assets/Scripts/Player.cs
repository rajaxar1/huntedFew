using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool jump = true;

    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 800;

    public Transform groundCheck;
    float groundRadius = 0.2f;
    public LayerMask whatIsGround;

    bool grounded = false;
	private Rigidbody2D rb;

	int coins = 0;
	Vector3 startingPosition;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		startingPosition = transform.position;
	}

   /* void OnTriggerEnter2D (Collider2D other){
        if (other.gameObject.tag == "MainCamera"){
            cameras.CameraSwamp(other);
        }
    }*/

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) /*&& grounded*/)
        {   
            if (grounded){
                rb.AddForce(new Vector2(0f, jumpForce));
                jump = true;
            }
            else {
                rb.AddForce(new Vector2(0f, 600));
                jump = true;
            }
        }
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

        if (!grounded) {

            float h = Input.GetAxis("Horizontal");

            if(h * rb.velocity.x < maxSpeed)
                rb.AddForce(Vector2.right * h * 100f);
            
            if (Mathf.Abs(rb.velocity.x) > maxSpeed)
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);

            if (h > 0 && !facingRight)
                Flip();
            else if (h < 0 && facingRight)
                Flip();

        }

        else{

            float h = Input.GetAxis("Horizontal");

            if(h * rb.velocity.x < maxSpeed)
                rb.AddForce(Vector2.right * h * moveForce);
            
            if (Mathf.Abs(rb.velocity.x) > maxSpeed)
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);

            if (h > 0 && !facingRight)
                Flip();
            else if (h < 0 && facingRight)
                Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
