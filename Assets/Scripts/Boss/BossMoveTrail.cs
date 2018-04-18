using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoveTrail : MonoBehaviour
{

    public int movementSpeed = 230;
    public int Damage = 10;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * movementSpeed);
        Destroy(this.gameObject, 0.2f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("hit player");
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                Debug.Log("shot player");
                playerHealth.TakeDamage(Damage);
            }
        }
    }
}
