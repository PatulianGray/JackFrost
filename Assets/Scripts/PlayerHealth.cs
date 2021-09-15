using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int hearthNumber;

    public GameObject deathScreen;
    public GameObject blood;
    public Transform bloodPoint;
    public Transform animBody;

    public Image[] hearts;
    public Sprite fullHearts;
    public Sprite emptyHearts;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            TakeDamage(1);
        }

        if (health > hearthNumber)
        {
            health = hearthNumber;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHearts;
            }
            else
            {
                hearts[i].sprite = emptyHearts;
            }
            if (i < hearthNumber)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animBody.position = this.transform.position;
        deathScreen.SetActive(true);
        this.gameObject.SetActive(false);
        Instantiate(blood, bloodPoint.position, bloodPoint.rotation);
    }
}
