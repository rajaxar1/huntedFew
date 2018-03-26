using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public float fireRate = 0.0f;
    public float damage = 10.0f;
    public LayerMask notToHit;

    private float timeToFire = 0.0f;
    private Transform firePoint;

	// Use this for initialization
	void Awake () {
        firePoint = transform.Find("FirePoint");
        if(firePoint == null)
        {
            Debug.LogError("No firePoint");
        }


	}
	
	// Update is called once per frame
	void Update () {
		if(fireRate <= 0.1f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
            else
            {
                if (Input.GetButton("Fire1") && Time.time > timeToFire)
                {
                    timeToFire = Time.time + 1 / fireRate;
                    Shoot();
                }
            }
        }
	}

    private void Shoot()
    {
        throw new NotImplementedException();
    }
}
