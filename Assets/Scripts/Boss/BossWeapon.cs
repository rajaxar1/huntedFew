using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapon : MonoBehaviour
{

    public float fireRate = 0.0f;
    public int damage = 10;
    public LayerMask whatToHit;
    public Transform BulletTrailPrefab;
    public Transform MuzzleFlashPrefab;

    GameObject player;

    private float TimeToSpawnEffect = 0;
    public float EffectSpawnRate = 10;
    private float timeToFire = 0.0f;
    private Transform firePoint;

    // Use this for initialization
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        this.gameObject.SetActive(false);

        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("No firePoint");
        }


    }

    void Update()
    {
        if (Time.time > timeToFire)
        {
            timeToFire = Time.time + 1 / fireRate;
            Shoot();
            this.gameObject.SetActive(false);
        }
        
    }

    private void Shoot()
    {
        Vector2 playerPosition = player.transform.position + new Vector3(0, 2, 0);
        Vector2 firePointPostion = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit;

        hit = Physics2D.Raycast(firePointPostion, playerPosition - firePointPostion, 100.0f, whatToHit);        

        if (Time.time >= TimeToSpawnEffect)
        {
            Effect();
            TimeToSpawnEffect = Time.time + 1 / EffectSpawnRate;
        }
    }

    private void Effect()
    {
        Vector3 playerPosition = player.transform.position;

        var pos = Camera.main.WorldToScreenPoint(transform.position);
        var dir = Camera.main.WorldToScreenPoint(playerPosition + new Vector3(0, 2, 0)) - pos;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Instantiate(BulletTrailPrefab, firePoint.position, Quaternion.AngleAxis(angle, Vector3.forward));
        

        //Transform effectInstance = (Transform)Instantiate(MuzzleFlashPrefab, firePoint.position, firePoint.rotation);

        //effectInstance.parent = firePoint;
        //float size = UnityEngine.Random.Range(3.0f, 5.0f);
        //effectInstance.localScale = new Vector3(size, size, size);

        //Destroy(effectInstance.gameObject, 0.02f);
    }
}
