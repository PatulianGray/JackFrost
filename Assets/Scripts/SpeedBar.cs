using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public GameObject sliderUI;

    public float maxSpeed = 10f;
    public float minSpeed = 0f;
    public float currentSpeed;
    public float speedMultiplier = 2;

    private void Start()
    {
        slider.maxValue = maxSpeed;
        currentSpeed = GetComponent<RBController>().defaultSpeed;
        slider.value = currentSpeed;
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && currentSpeed < 10)
        {
            currentSpeed += Time.deltaTime * speedMultiplier;
            slider.value = currentSpeed;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && currentSpeed >= 0)
        {
            currentSpeed -= Time.deltaTime * speedMultiplier;
            slider.value = currentSpeed;
        }
    }
}
