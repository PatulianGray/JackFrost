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

    // ��� ���� �� ��� ������, ��� ������ ����, ����� ��������� ���������, �� ����� �������� ���������, �����-�� �����, �� ����� ������� ���������� �� ����,
    // ��� ��������� �������� �����. � ����� ��� �������� � ����, ��� ��������� ��������, ���� ����������.
    //�� � ������ ������ ��, ����� �� ���, ����� ��� ����� ����������.

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
