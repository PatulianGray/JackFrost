using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float disappearlDelay = 2f;
    public float appearDelay = 4f;

    public float shakeSpeed;
    public Vector3 originPosition;

    public GameObject platform;

    void Start()
    {
        originPosition = transform.position;
    }

    // “ут было бы все хорошо, вот только если, когда платформа отключена, ты снова заденешь коллайдер, каким-то боком, то отчет задержи запуститс€ до того,
    // как платформа по€витс€ вновь. ¬ итоге это приведет к тому, что плотформа исчезнет, едва по€вившись.
    //ƒа и вообще пон€ть бы, нужно ли нам, чтобы она снова по€вл€лась.

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = transform;

            float step = shakeSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, originPosition + Random.insideUnitSphere, step);
            StartCoroutine(Disappear());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;

        StartCoroutine(Appear());
    }


    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(disappearlDelay);
        platform.gameObject.SetActive(false);
        yield return 0;
    }

    IEnumerator Appear()
    {
        yield return new WaitForSeconds(appearDelay);
        platform.gameObject.SetActive(true);
        yield return 0;
    }

}
