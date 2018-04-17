using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrail : MonoBehaviour {

    public int movementSpeed = 230;
    public int Damage = 10;
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.right * Time.deltaTime * movementSpeed);
        Destroy(this.gameObject, 0.2f);
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        Boss bossEnemy = collision.GetComponent<Boss>();
        if (bossEnemy != null)
        {
            Debug.Log(bossEnemy.bossStats.health);
            bossEnemy.DamageBoss(Damage);
        }
    }
}
