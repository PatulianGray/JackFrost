using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObstacle : MonoBehaviour
{
    public GameObject explosion;
    public Transform explosionPoint;
    public float impulceForce = 500;
    public float impulceForceUp = 50;
    public float regainCOntrolTime = 0.2f;
    public int damage = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider hitInfo)
    {
        if (hitInfo.CompareTag("Player"))
        {
            RBController Player = hitInfo.GetComponent<RBController>();
            if (Player.scaleFactor > 0.7f)
            {
                Die();
            }
            else
            {
                PlayerHealth player = hitInfo.GetComponent<PlayerHealth>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                }
                Player.enabled = false;
                StartCoroutine(EnableControl());
                Player.rb.AddForce(Vector3.left * impulceForce, ForceMode.Impulse);
                Player.rb.AddForce(Vector3.up * impulceForceUp, ForceMode.Impulse);
            }
            if (Player != null)
            {
                Debug.Log("Next Time");
            }
        }

    }

    void Die()
    {
        Destroy(gameObject);

        Instantiate(explosion, explosionPoint.position, explosionPoint.rotation);
    }

    private IEnumerator EnableControl()
    {
        yield return new WaitForSeconds(regainCOntrolTime);
        RBController Player = FindObjectOfType<RBController>();
        Player.enabled = true;
    }
}
