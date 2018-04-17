using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrail : MonoBehaviour {

    public int movementSpeed = 230;

	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.right * Time.deltaTime * movementSpeed);
        Destroy(this.gameObject, 0.2f);
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Destroy(this.gameObject);
    }
}
