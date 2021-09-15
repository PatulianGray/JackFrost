using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFloorFollow : MonoBehaviour
{
    public Transform pos;
    public float deathFloorPos;
    public int damage = 1;

    public bool deathFloor;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (deathFloor)
        {
            this.transform.position = new Vector3(pos.position.x, deathFloorPos, 0);
        }
        else
        {
            pos = null;
        }
    }

    void OnTriggerEnter(Collider hitInfo)
    {
        if (hitInfo.CompareTag("Player"))
        {
            PlayerHealth player = hitInfo.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }

    }
}
