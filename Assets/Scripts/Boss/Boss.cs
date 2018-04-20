using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    public BossStats bossStats = new BossStats();
    public Mover mover;
    public Transform BossWeaponPrefab;
    GameObject player;
    public static int shotCount = 0;

    PlayerHealth playerHealth;
    WeaponSwitching playerWeapon;
    Animator anim;
    bool playerInRange;
    float timer = 0.0f;

    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 25;

    public bool hasPistol = false;
    public bool hasAr = false;
    public bool hasRocket = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        playerWeapon = player.GetComponent<Player>().weaponSwitching;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If the entering collider is the player...
        if (other.gameObject.tag == "Player")
        {
            // ... the player is in range.
            playerInRange = true;
            
            if(!(hasPistol && hasAr && hasRocket)){ //check if boss knows player has all weapons
                if (playerWeapon != null)
                {
                    if (playerWeapon.selectedWeapon == 0)
                    { //store that boss knows about the pistol
                        hasPistol = true;
                    }
                    else if (playerWeapon.selectedWeapon == 1)
                    { //store that boss knows about the ar
                        hasAr = true;
                    }
                    else if (playerWeapon.selectedWeapon == 2)
                    { //store that boss knows about the rocket
                        hasRocket = true;
                    }
                }
            }
        }
        
        if(other.gameObject.tag == "Ladder"){
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
        
    }

    public float currentHealth()
    {
        return bossStats.health;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // If the exiting collider is the player...
        if (other.gameObject.tag == "Player")
        {
            // ... the player is no longer in range.
            playerInRange = false;
        }

        if(other.gameObject.tag == "Ladder"){
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 2;
        }
    }

    void Update()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
        if (timer >= timeBetweenAttacks)
        {
            Debug.Log("shot count "+shotCount);
            if (playerInRange){
                Attack();
            }
            if (shotCount > 2){
                Fire();
            }
        }
    }

    void Fire(){
        foreach (Transform weapon in transform)
        {
            if (weapon.name == "BossWeaponPrefab")
            {
                Debug.Log("activating boss weapon");
                weapon.gameObject.SetActive(true);
                shotCount--;
                weapon.gameObject.SetActive(true);
                shotCount--;
            }
        }
    }


    void Attack()
    {
        AnimTrigger("Attack");
        timer = 0f;

        if (playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage(attackDamage);
        }
        ExecuteAfterDelay();

        //Attack();
        //mover.SwitchToFleeState();
    }

    IEnumerator ExecuteAfterDelay(){
        yield return new WaitForSeconds(5);
    }

    void AnimTrigger(string triggerName)
    {
        foreach (AnimatorControllerParameter p in anim.parameters)
            if (p.type == AnimatorControllerParameterType.Trigger)
                anim.ResetTrigger(p.name);
        anim.SetTrigger(triggerName);
    }

    public void HangryBoss(){
        timeBetweenAttacks = .3f;
        attackDamage = 50;
    }

    public void DamageBoss(int damage)
    {
        shotCount++;
        bossStats.health -= damage;
        if (bossStats.health <= 50f){
            HangryBoss();
        }
        if (bossStats.health <= 0f)
        {
            anim.SetBool("Death", true);
            GameMaster.KillBoss(this);
        }
    }

    public static int setShotCount(){
        return shotCount; 
    }

    public class BossStats
    {
        public float health = 100f;

    }
}
