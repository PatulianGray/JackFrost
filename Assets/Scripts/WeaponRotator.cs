using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRotator : MonoBehaviour
{
    public bool clamp = false;
    public float minAngle = -60;
    public float maxAngle = 70;
    void Update()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = Input.mousePosition - pos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (clamp)
        {
            angle = Mathf.Clamp(angle, minAngle, maxAngle);
        }

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
