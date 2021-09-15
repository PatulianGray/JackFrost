using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBlast : MonoBehaviour
{
    [Range (1, 1000)] public float blastForce;

    public GameObject player;

    public Transform firePoint;
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 mousepos = Camera.main.WorldToScreenPoint(transform.position);
        if (Input.GetButtonDown("Fire1"))
        {
            var instance = Instantiate(player, firePoint.position, player.transform.rotation);
            instance.GetComponent<Rigidbody>().AddForce(mousepos * blastForce);
        }
    }
}
