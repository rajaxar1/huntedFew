using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour {

    public BossStats bossStats = new BossStats();

    GameObject player;

    PlayerHealth playerHealth;

    bool playerInRange;
    float timer;

    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.tag);
        // If the entering collider is the player...
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("hitting player");
            // ... the player is in range.
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // If the exiting collider is the player...
        if (other.gameObject.tag == "Player")
        {
            // ... the player is no longer in range.
            playerInRange = false;
        }
    }

    void Update()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
        if (timer >= timeBetweenAttacks && playerInRange)
        {
            // ... attack.
            Attack();
        }
    }


    void Attack()
    {
        timer = 0f;

        if (playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }

    public void DamageBoss(int damage)
    {
        bossStats.health -= damage;
        if (bossStats.health <= 0f)
        {
            GameMaster.KillBoss(this);
        }
    }

    public class BossStats
    {
        public float health = 100f;

    }
}
