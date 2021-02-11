using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoKartController : MonoBehaviour
{
    //TODO:
    //Magic numbers 1000f  & 100f could be a static float
    //In rotation get the actual speed instead of input, you should be able to turn if you have speed
    //Remove as many public variables as possible
    //Make camera follow
    //Make cool track?

    public Rigidbody SphereRigidbody;
    public float ForwardAcceleration = 8f;
    public float ReverseAcceleration = 4f;
    //public float MaxSpeed = 50f;
    public float TurnStrength = 180f;
    public float GravityForce = 10f;
    public float DragOnGround = 3f;

    private float speedInput, turnInput;

    private bool grounded;
    public LayerMask WhatIsGround;
    public float GroundRayLength = 0.5f;
    public Transform GroundRayPoint;

    void Start()
    {
        SphereRigidbody.transform.parent = null;
    }

    void Update()
    {
        HandleInput();

        if (grounded)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * TurnStrength * Time.deltaTime * Input.GetAxis("Vertical"), 0f));
        }

        Vector3 offsetSphereTransform = SphereRigidbody.transform.position - new Vector3(0, 0.45f, 0);
        transform.position = offsetSphereTransform;
        //transform.position = SphereRigidbody.transform.position;
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    private void HandleInput()
    {
        speedInput = 0f;
        if (Input.GetAxis("Vertical") > 0)
        {
            speedInput = Input.GetAxis("Vertical") * ForwardAcceleration * 1000f;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            speedInput = Input.GetAxis("Vertical") * ReverseAcceleration * 1000f;
        }

        turnInput = Input.GetAxis("Horizontal");
    }


    private void GroundCheck()
    {
        grounded = false;
        RaycastHit hit;

        if (Physics.Raycast(GroundRayPoint.position, -transform.up, out hit, GroundRayLength, WhatIsGround))
        {
            grounded = true;

            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }

        if (grounded)
        {
            SphereRigidbody.drag = DragOnGround;

            if (Mathf.Abs(speedInput) > 0)
            {

                SphereRigidbody.AddForce(transform.forward * speedInput);
            }
        }
        else
        {
            SphereRigidbody.drag = 0.1f;
            SphereRigidbody.AddForce(Vector3.up * -GravityForce * 100f);
        }
    }
}
