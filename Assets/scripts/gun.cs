﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class gun : MonoBehaviour
{

    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public float fireRate = 15f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public Animator shootAnim;

    private float NextTimeToFire = 0f;

    //public GunState gunstate;

    public bool isShooting;
    public shakeBehavior shaker;

    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;

    //public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading) 
            return;

        isShooting = false;

        if(currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (CanShoot())
        {
            
            NextTimeToFire = Time.time + 1f / fireRate;

            shoot();
            //shootAnim.SetTrigger("shoot");
        }
        



    }

    bool CanShoot()
    {
        return Input.GetMouseButton(0) && Time.time >= NextTimeToFire;
    }
    
    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;

    }

    void LateUpdate()
    {
        

        if (CanShoot())
        {
            isShooting = true;
            

            
            //shootAnim.SetTrigger("shoot");
        }




    }

    void shoot()
    {
        //var newbullet = Instantiate(bulletPrefab, (transform.position + transform.up * 1.5f + transform.forward), Quaternion.identity);
        // Destroy(newbullet, 2f);
        shaker.Triggershake();
        muzzleFlash.Play();

        currentAmmo--;
        

        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

           target target = hit.transform.GetComponent<target>();
            if(target != null)
            {
                target.TakeDamage(damage);
            }

            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }
    }
}
