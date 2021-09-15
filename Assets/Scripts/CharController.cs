using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;

    [SerializeField] private Transform[] groundChecks;
    [SerializeField] private Transform[] wallChecks;

    private float gravity = -50f;
    private float jumpBufferCount;

    [SerializeField] [Range(-1, 1)] private float horizontalInput;
    [SerializeField] [Range(1, 10)] private float runSpeed = 5f;
    [SerializeField] [Range(1, 10)] private float jumpHeight;
    [SerializeField] [Range(1, 10)] private float wallJumpHeight = 7f;
    [SerializeField] [Range(1, 10)] private float minJumpHeight = 1f;
    [SerializeField] [Range(1, 10)] private float maxJumpHeight = 10f;
    [SerializeField] [Range(1, 10)] private float jumpChargeSpeed = 2f;

    [SerializeField] [Range(0.1f, 1)] private float groundCheckRadius = 0.1f;
    [SerializeField] [Range(0.1f, 1)] private float wallCheckRadius = 0.1f;
    [SerializeField] private float jumpBufferLenght = 0.1f;

    private bool isGrounded;
    private bool wallNearby;
    private bool sideSwitch;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        horizontalInput = sideSwitch ? -1 : 1;

        if (isGrounded)
        {
            sideSwitch = false;
        }
        //Face forward
        transform.forward = new Vector3(horizontalInput, 0, Mathf.Abs(horizontalInput) - 1);

        //Is grounded
        isGrounded = false;
        foreach (var groundCheck in groundChecks)
        {
            if (isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer, QueryTriggerInteraction.Ignore))
            {
                isGrounded = true;
                break;
            }
        }

        wallNearby = false;
        foreach (var wallCheck in wallChecks)
        {
            if (wallNearby = Physics.CheckSphere(wallCheck.position, wallCheckRadius, wallLayer, QueryTriggerInteraction.Ignore))
            {
                wallNearby = true;
                break;
            }
        }

        //Zeroing gravity when grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }
        else
        {
            //Gravity
            velocity.y += gravity * Time.deltaTime;
        }

        //Horizontal velocity
        controller.Move(new Vector3(horizontalInput * runSpeed, 0, 0) * Time.deltaTime);

        //Jump command buffer
        if (Input.GetButtonUp("Fire2"))
        {
            jumpBufferCount = jumpBufferLenght;
        }
        else
        {
            jumpBufferCount -= Time.deltaTime;
        }

        //Jump height buffer multiplier
        if (Input.GetButton("Fire2") && isGrounded)
        {
            if (jumpHeight < maxJumpHeight)
            {
                jumpHeight += Time.deltaTime * jumpChargeSpeed;
            }
            print(jumpHeight);
        }
        else if (!isGrounded)
        {
            jumpHeight = minJumpHeight;
        }

        if (isGrounded && jumpBufferCount >= 0)
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
            jumpBufferCount = 0;
        }
        //Vertical velocity
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ResetPosition();
        }
    }

    void OnDrawGizmos()
    {
        foreach (var groundCheck in groundChecks)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        foreach (var wallCheck in wallChecks)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!isGrounded && wallNearby)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                velocity.y = 0;
                sideSwitch = !sideSwitch;
                velocity.y += Mathf.Sqrt(wallJumpHeight * -2 * gravity);
                jumpBufferCount = 0;
                Debug.DrawRay(hit.point, hit.normal, Color.red, 1.25f);
            }
        }
    }

    public void ResetPosition()
    {
        controller.enabled = false;
        this.transform.position = Vector3.zero;
        controller.enabled = true;
    }
}
