using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool jump = true;

    public float speed = 10f;
    public float maxSpeed = 5f;
    public float jumpForce = 700;

    public Transform groundCheck;
    float groundRadius = 0.2f;
    public LayerMask whatIsGround;

    static Animator anim;

    bool grounded = false;
    private Rigidbody2D rb;

    int coins = 0;
    Vector3 startingPosition;

    int numberOfJumps = 0;
    bool onLadder = false;
    bool hasRocketLauncher = false;
    bool hasMachineGun = false;
    bool hasPistol = false;
    int ammoClips = 1;
    int rockets = 10;
    float default_grav = 0;

    private bool inTrigger = false;
    private Transform gun;

    public WeaponSwitching weaponSwitching;


    // Use this for initialization
    void Start() {
        anim = GetComponent<Animator>();
        anim.SetBool("melee", true);
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 10;
        startingPosition = transform.position;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ladder") { onLadder = true; default_grav = rb.gravityScale; rb.gravityScale = 0; }
        inTrigger = true;
        gun = collision.transform;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ladder") { onLadder = false; rb.gravityScale = default_grav; }
        inTrigger = false;
    }



    void Update()
    {
        if (inTrigger && gun != null) {
            switch (gun.tag)
            {
                case "AssaultRifle":
                    hasMachineGun = true; // enable switching to the machine gun
                    Destroy(gun.gameObject); // destroy the picked object
                    weaponSwitching.AddItem("ar");
                    break;
                case "RocketLauncher":
                    hasRocketLauncher = true; // enable switching to the rocket launcher
                    Destroy(gun.gameObject); // destroy the object
                    weaponSwitching.AddItem("rocket");
                    break;
                case "Pistol":
                    hasPistol = true;
                    Destroy(gun.gameObject);
                    weaponSwitching.AddItem("pistol");
                    break;
                case "AmmoClip":
                    ammoClips++; // increment ammo clips
                    Destroy(gun.gameObject);
                    break;
                case "Rocket":
                    rockets++; // increment rockets
                    Destroy(gun.gameObject);
                    break;
                case "ShootAnywhere":
                    Weapon.setShootAnywhere();
                    Destroy(gun.gameObject);
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            AnimTrigger("punch");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            AnimTrigger("kick");
        }

        else if (onLadder) {
            if (Input.GetAxis("Vertical") > 0) {
                transform.Translate(0, 1 * Time.deltaTime, 0);
            }
            if (Input.GetAxis("Vertical") < 0) {
                transform.Translate(0, -1 * Time.deltaTime, 0);
            }
        }
    }

    public static void setActiveGun(string gun)
    {
        switch (gun)
        {
            case "ArPrefab(Clone)":
                setArActive();
                break;
            case "PistolPrefab(Clone)":
                setPistolActive();
                break;
            case "RocketPrefab(Clone)":
                setRocketActive();
                break;
        }
    }

    private static void setArActive()
    {
        anim.SetBool("pistol", false);
        anim.SetBool("ar", true);
        anim.SetBool("melee", false);
        anim.SetBool("rocket", false);
    }

    private static void setPistolActive()
    {
        anim.SetBool("pistol", true);
        anim.SetBool("ar", false);
        anim.SetBool("melee", false);
        anim.SetBool("rocket", false);
    }

    private static void setRocketActive()
    {
        anim.SetBool("pistol", false);
        anim.SetBool("ar", false);
        anim.SetBool("melee", false);
        anim.SetBool("rocket", true);
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

        if (grounded) numberOfJumps = 0;

        if (Input.GetKeyDown(KeyCode.Space) && (numberOfJumps < 2 || grounded))
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            numberOfJumps++;
            jump = true;
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

        rb.velocity = new Vector2(rb.velocity.x + (movement.x * speed), rb.velocity.y); 

        if(Mathf.Abs(rb.velocity.x) > maxSpeed)
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);

        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
             rb.velocity = new Vector2(0, rb.velocity.y);

        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));

        if (moveHorizontal > 0 && !facingRight)
            Flip();
        else if (moveHorizontal < 0 && facingRight)
            Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        //Vector3 theScale = transform.localScale;
        //theScale.x *= -1;
        transform.Rotate(new Vector3(0f, 180f, 0f));
        //transform.localScale = theScale;
    }

}
