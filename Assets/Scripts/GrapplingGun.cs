using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    private Rigidbody rb;

    private LineRenderer lr;
    private RBController rbController;
    private Vector3 grapplePoint;
    private SpringJoint joint;
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private Transform firePoint, player;

    [SerializeField] [Range (1, 100)] private float speed = 5f;
    [SerializeField] [Range (1, 100)] private float aimAssistRadius = 1f;

    public bool canGrapple;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
        rbController = GetComponent<RBController>();
        lr.enabled = false;
    }

    void Update()
    {
        //Fire grappling gun
        if (Input.GetButtonDown("Fire2") && canGrapple)
        {
            StartGrapple();
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            StopGrapple();
        }
    }
    void LateUpdate()
    {
        DrawRope();
    }

    void StartGrapple()
    {
        //Upon executing, creates a spring joint and a line renderer, disable character controller
        //and gives a Player an initial forward velocity

        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Physics.SphereCast(ray, aimAssistRadius, out hit, grappleLayer)
        //Physics.Raycast(firePoint.position, firePoint.forward, out hit, 20f, grappleLayer)

        if (Physics.SphereCast(ray, aimAssistRadius, out hit, 1000, grappleLayer))
        {
            Debug.Log("Grapping");
            lr.enabled = true;
            rbController.enabled = false;
            rb.velocity = new Vector3(1 * speed, rb.velocity.y);
            grapplePoint = hit.point;

            //Spring joint settings
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 100f;
            joint.damper = 10f;
            joint.massScale = 20f;

            lr.positionCount = 2;
        }
    }
    void StopGrapple()
    {
        //Destroys joint upon executing, enables controller and disables line renderer
        lr.enabled = false;
        rbController.enabled = true;
        lr.positionCount = 0;
        Destroy(joint);
    }

    void DrawRope()
    {
        //Draws black line from firing position to clicked object
        if (!joint) return;
        lr.SetPosition(index: 0, firePoint.position);
        lr.SetPosition(index: 1, grapplePoint);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Grapple"))
        {
            canGrapple = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grapple"))
        {
            canGrapple = false;
        }
    }
}
