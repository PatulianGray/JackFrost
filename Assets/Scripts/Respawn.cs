using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject player;
    public GameObject deathScreen;
    public Transform respawnPoint;
    // Start is called before the first frame update
    public void RespawnPlayer()
    {
        player.GetComponent<PlayerHealth>().health = 5;
        player.GetComponent<PlayerHealth>().animBody.position = player.transform.position;
        player.SetActive(true);
        deathScreen.SetActive(false);
        player.transform.position = respawnPoint.position;
    }
}
