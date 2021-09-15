using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 10;
    public Rigidbody rb;

    public GameObject explosion;
    void Start()
    {
        rb.velocity = transform.forward * speed;
        StartCoroutine(BulletDestroyOverTime());
    }

    void OnTriggerEnter(Collider hitInfo)
    {
        if (!hitInfo.CompareTag("Grapple") && !hitInfo.CompareTag("IgnoreBullets"))
        {
            Instantiate(explosion, this.transform.position, this.transform.rotation);
            Enemy enemy = hitInfo.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

    }

    private IEnumerator BulletDestroyOverTime()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
