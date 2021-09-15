using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int currnetHealth;
    public int maxHealth = 100;

    public HealthBar healthBar;
    public GameObject explosion;
    public Transform explosionPoint;

    public MeshRenderer mesh;
    public Color damageTaken;

    private void Start()
    {
        currnetHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        StartCoroutine(Flash());

        currnetHealth -= damage;

        healthBar.SetHealth(currnetHealth);

        if (currnetHealth <= 0)
        {
            Die();
        }
    }
    private IEnumerator Flash()
    {
        mesh.material.color = damageTaken;

        yield return new WaitForSeconds(0.05f);

        mesh.material.color = new Color32(192, 0, 0, 255);
    }

    void Die()
    {
        Destroy(gameObject);

        Instantiate(explosion, explosionPoint.position, explosionPoint.rotation);
    }

}
