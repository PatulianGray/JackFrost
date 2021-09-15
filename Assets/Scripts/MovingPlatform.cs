using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3[] points;
    public int pointNumber = 0;
    private Vector3 currnetTarget;

    public float tolerance;
    public float speed;
    public float delayTime;

    private float delayStart;
    public bool automatic;
    public bool saw;

    void Start()
    {
        if (points.Length > 0)
        {
            currnetTarget = points[0];
        }

        tolerance = speed * Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (transform.localPosition != currnetTarget)
        {
            MovePlatform();
        }
        else
        {
            UpdateTarget();
        }
    }

    void MovePlatform()
    {

        Vector3 heading = currnetTarget - transform.localPosition;
        transform.localPosition += (heading / heading.magnitude) * speed * Time.deltaTime;
        if (heading.magnitude < tolerance)
        {
            transform.localPosition = currnetTarget;
            delayStart = Time.time;
        }

    }

    void UpdateTarget()
    {

        if (automatic)
        {
            if (Time.time - delayStart > delayTime)
            {
                NextPlatform();
            }
        }

    }

    public void NextPlatform()
    {

        pointNumber++;
        if (pointNumber >= points.Length)
        {
            pointNumber = 0;
        }

        currnetTarget = points[pointNumber];

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!saw)
        {
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!saw)
        {
            other.transform.parent = null;
        }
    }
}
