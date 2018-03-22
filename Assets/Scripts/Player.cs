using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool jump = true;

    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 700;

    public Transform groundCheck;
    float groundRadius = 0.2f;
    public LayerMask whatIsGround;

    Animator anim;

    bool grounded = false;
	private Rigidbody2D rb;

	int coins = 0;
	Vector3 startingPosition;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        anim.SetBool("melee", true);
		rb = GetComponent<Rigidbody2D>();
		startingPosition = transform.position;
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            jump = true;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            AnimTrigger("punch");     
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            AnimTrigger("kick");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            anim.SetBool("pistol", true);
            anim.SetBool("ar", false);
            anim.SetBool("melee", false);
            anim.SetBool("rocket", false);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            anim.SetBool("pistol", false);
            anim.SetBool("ar", true);
            anim.SetBool("melee", false);
            anim.SetBool("rocket", false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            anim.SetBool("pistol", false);
            anim.SetBool("ar", false);
            anim.SetBool("melee", true);
            anim.SetBool("rocket", false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            anim.SetBool("pistol", false);
            anim.SetBool("ar", false);
            anim.SetBool("melee", false);
            anim.SetBool("rocket", true);
        }
    }

    void AnimTrigger(string triggerName)
    {
        foreach (AnimatorControllerParameter p in anim.parameters)
            if (p.type == AnimatorControllerParameterType.Trigger)
                anim.ResetTrigger(p.name);
        anim.SetTrigger(triggerName);
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

        if (!grounded) return;


        float move = Input.GetAxis("Horizontal");

        if(move * rb.velocity.x < maxSpeed)
            rb.AddForce(Vector2.right * move * moveForce);
        
        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);

        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));

        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
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
