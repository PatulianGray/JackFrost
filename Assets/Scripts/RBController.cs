using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBController : MonoBehaviour
{
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Animator anim;

    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;

    [SerializeField] private Transform[] groundChecks;
    [SerializeField] private Transform[] wallChecks;
    [SerializeField] private Transform[] wallJumpChecks;

    private float hInput;
    private float jumpBufferCount;

    [Range(0, 10)] public float defaultSpeed = 5f;
    [SerializeField] private float speed;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float speedDivider = 2f;
    [SerializeField] [Range(0, 10)] private float wallJumpSpeed = 10f;

    [SerializeField] private float jumpTransitionInTime = 0.1f;
    [SerializeField] private float jumpTransitionOutTime = 1f;

    [Range(1, 100)] public float jumpForce = 10f;
    [SerializeField] [Range(1, 100)] private float minJumpForce = 1f;
    [SerializeField] [Range(1, 100)] private float maxJumpForce = 10f;
    [SerializeField] [Range(1, 10)] private float jumpChargeSpeed = 2f;
    [SerializeField] [Range(1, 10)] private float fallMultiplier = 2.5f;
    [SerializeField] [Range(-10, 0)] private float wallSlidingSpeed = -1f;
    [SerializeField] [Range(1, 100)] private float xWallForce = 2f;
    [SerializeField] [Range(1, 100)] private float yWallForce = 2f;

    [SerializeField] [Range(0.1f, 1)] private float groundCheckRadius = 0.1f;
    [SerializeField] [Range(0.1f, 1)] private float wallCheckRadius = 0.1f;
    [SerializeField] [Range(0.1f, 1)] private float wallJumpCheckRadius = 0.1f;

    [SerializeField] private float dashTime = 0.5f;
    [SerializeField] private float dashSpeedX = 10f;
    [SerializeField] private float dashSpeedY = 2f;

    [SerializeField] private float jumpBufferLenght = 0.1f;

    [HideInInspector] public bool isGrounded;
    private bool wallNearby;
    private bool wallJumping;
    private bool wallSliding;
    private bool sideSwitch;
    public bool isDashing;
    public bool canDash;
    public bool wasOnGround;

    public MeshRenderer mesh;
    public Color fatige;

    public ParticleSystem footsteps;
    public ParticleSystem impactEffect;
    public ParticleSystem.EmissionModule footEmission;

    void Awake()
    {
        footEmission = footsteps.emission;
        speed = defaultSpeed;
        jumpForce = minJumpForce;
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        //Change speed with mouse scroll wheel
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && defaultSpeed < 10)
        {
            defaultSpeed += Time.deltaTime * speedMultiplier;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && defaultSpeed >= 0)
        {
            defaultSpeed -= Time.deltaTime * speedMultiplier;
        }

        //Dashing
        if (!isGrounded && Input.GetButton("Fire2") && Input.GetButtonDown("Fire1") && canDash)
        {
            Debug.Log("Dash");
            isDashing = true;
            canDash = false;
            StartCoroutine(DashCancel());
        }

        if (isDashing)
        {
            mesh.material.color = fatige;
        }

        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isDashing", isDashing);

        //Automaticly switches sides, when walljumping
        hInput = sideSwitch ? -1 : 1;

        //hInput = Input.GetAxisRaw("Horizontal");

        if (isGrounded)
        {
            sideSwitch = false;
            canDash = true;
            mesh.material.color = new Color32(30, 204, 0, 255);
        }

        //Facing forward
        transform.forward = new Vector3(hInput, 0, Mathf.Abs(hInput) - 1);

        //Movement
        rb.velocity = new Vector3(isDashing ? hInput * dashSpeedX : hInput * speed, isDashing ? dashSpeedY : rb.velocity.y);

        //Ground check
        isGrounded = false;

        foreach (var groundCheck in groundChecks)
        {
            if (isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer, QueryTriggerInteraction.Ignore))
            {
                isGrounded = true;
                break;
            }
        }

        //Wall check
        wallNearby = false;
        foreach (var wallCheck in wallChecks)
        {
            if (wallNearby = Physics.CheckSphere(wallCheck.position, wallCheckRadius, wallLayer, QueryTriggerInteraction.Ignore))
            {
                wallNearby = true;
                break;
            }
        }

        //Wall jump check
        wallJumping = false;
        foreach (var wallJumpCheck in wallJumpChecks)
        {
            if (wallJumping = Physics.CheckSphere(wallJumpCheck.position, wallJumpCheckRadius, wallLayer, QueryTriggerInteraction.Ignore))
            {
                wallJumping = true;
                break;
            }
        }

        if (wallJumping && !isGrounded)
        {
            speed = Mathf.Lerp(speed, wallJumpSpeed, jumpTransitionInTime);
        }
        else
        {
            speed = Mathf.Lerp(speed, defaultSpeed, jumpTransitionOutTime); ;
        }

        //Fall velocity multiplier
        if (rb.velocity.y < 0)
        {
            anim.SetFloat("speedY", -1, 0.35f, Time.deltaTime);
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        if (rb.velocity.y > 0)
        {
            anim.SetFloat("speedY", 1 , 0.05f, Time.deltaTime);
        }

        //Jump command buffer
        if (Input.GetButtonUp("Fire2"))
        {
            speed = defaultSpeed;
            anim.SetBool("isJumping", false);
            jumpBufferCount = jumpBufferLenght;
        }
        else
        {
            jumpBufferCount -= Time.deltaTime;
        }

        //Jump height buffer multiplier
        if (Input.GetButton("Fire2") && isGrounded)
        {
            speed = defaultSpeed / speedDivider;
            anim.SetBool("isJumping", true);
            if (jumpForce < maxJumpForce)
            {
                jumpForce += Time.deltaTime * jumpChargeSpeed;
            }
        }
        else if (!isGrounded)
        {
            jumpForce = minJumpForce;
        }

        if (isGrounded && jumpBufferCount >= 0)
        {
            rb.velocity = Vector3.up * jumpForce;
            jumpBufferCount = 0;
        }

        //Wall slide
        if (wallNearby && !isGrounded)
        {
            wallSliding = true;
        }
        else
        {
            wallSliding = false;
        }

        if (wallSliding)
        {
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, wallSlidingSpeed, float.MaxValue));
        }

        //Wall jump
        if (Input.GetButtonDown("Fire2") && wallSliding)
        {
            sideSwitch = !sideSwitch;
            rb.velocity = new Vector3(xWallForce * hInput, yWallForce);
        }

        //Footstep particles
        if (hInput != 0 && isGrounded)
        {
            footEmission.rateOverTime = 100f;
        }
        else
        {
            footEmission.rateOverTime = 0f;
        }

        //Impact effect
        if (!wasOnGround && isGrounded)
        {
            impactEffect.gameObject.SetActive(true);
            impactEffect.Stop();
            //impactEffect.transform.position = footsteps.transform.position;
            impactEffect.Play();
        }

        wasOnGround = isGrounded;
    }

    private IEnumerator DashCancel()
    {
        yield return new WaitForSeconds(dashTime);

        isDashing = false;
    }

    void OnDrawGizmos()
    {
        foreach (var groundCheck in groundChecks)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
        }
        foreach (var wallCheck in wallChecks)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(wallCheck.position, wallCheckRadius);
        }
        foreach (var wallJumpCheck in wallJumpChecks)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(wallJumpCheck.position, wallJumpCheckRadius);
        }
    }
}
