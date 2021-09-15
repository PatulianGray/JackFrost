using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpBar : MonoBehaviour
{
    public RBController rbc;
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public GameObject sliderUI;

    public float maxJump = 10f;
    public float minJump = 0f;
    public float currentJump;
    public float jumpChargeSpeed = 2;

    private void Start()
    {
        slider.maxValue = maxJump;
        slider.value = currentJump;
        rbc = GetComponent<RBController>();
    }

    private void Update()
    {
        if (Input.GetButton("Fire2") && currentJump < maxJump && rbc.isGrounded)
        {
            currentJump += Time.deltaTime * jumpChargeSpeed;
            slider.value = currentJump;
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            slider.value = minJump;
            currentJump = minJump;
        }
    }
}
