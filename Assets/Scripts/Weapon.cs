using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public float fireRate = 0.0f;
    public int damage = 10;
    public LayerMask whatToHit;
    public Transform BulletTrailPrefab;
    public Transform MuzzleFlashPrefab;

    private float TimeToSpawnEffect = 0;
    public float EffectSpawnRate = 10;
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
        if (fireRate <= 0.1f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
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

    private void Shoot()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
            Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPostion = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPostion, mousePosition - firePointPostion, 100, whatToHit);
        if (Time.time >= TimeToSpawnEffect)
        {
            Effect();
            TimeToSpawnEffect = Time.time + 1 / EffectSpawnRate;
        }
        Debug.DrawLine(firePointPostion, (mousePosition - firePointPostion) * 100, Color.cyan);

        if(hit.collider != null)
        {
            Debug.DrawLine(firePointPostion, hit.point, Color.red);
            BossEnemy bossEnemy = hit.collider.GetComponent<BossEnemy>();
            if (bossEnemy != null)
            {
                Debug.Log(bossEnemy.bossStats.health);
                bossEnemy.DamageBoss(damage);
            }
        }
    }

    private void Effect()
    {
        Instantiate(BulletTrailPrefab, firePoint.position, firePoint.rotation);
        Transform effectInstance = (Transform)Instantiate(MuzzleFlashPrefab, firePoint.position, firePoint.rotation);

        effectInstance.parent = firePoint;
        float size = UnityEngine.Random.Range(0.6f, 0.9f);
        effectInstance.localScale = new Vector3(size, size, size);

        Destroy(effectInstance.gameObject, 0.02f);
    }
}
