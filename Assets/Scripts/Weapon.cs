using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject muzzleFlash;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !Input.GetButton("Fire2"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Instantiate(muzzleFlash, firePoint.position, firePoint.rotation);
    }
}
