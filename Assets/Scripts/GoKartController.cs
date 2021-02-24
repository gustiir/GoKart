using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoKartController : MonoBehaviour
{
    //TODO:
    //Make cool track?
    //*Change FOV depending on Distance from cam to kart
    //*Magic numbers 1000f  & 100f could be a const float
    //*In rotation get the actual speed instead of input, you should be able to turn if you have speed
    //*Remove as many public variables as possible
    //*Make camera follow
    //*Add force parallel to the ground

    [SerializeField]
    private Rigidbody SphereRigidbody;
    [SerializeField]
    private float ForwardAcceleration = 8f;
    [SerializeField]
    private float ReverseAcceleration = 4f;
    [SerializeField]
    private float TurnStrength = 180f;
    [SerializeField]
    private float GravityForce = 10f;
    [SerializeField]
    private float DragOnGround = 3f;

    private float speedInput, turnInput;

    private const float accelerationMultiplier = 1000f;
    private const float forceMultiplier = 100f;

    [SerializeField]
    private bool grounded;
    [SerializeField]
    private LayerMask WhatIsGround;
    [SerializeField]
    private float GroundRayLength = 0.5f;
    [SerializeField]
    private Transform GroundRayPoint;

    [SerializeField]
    private Vector3 currentPosition;
    [SerializeField]
    private Vector3 previousPosition;

    void Start()
    {
        SphereRigidbody.transform.parent = null;
        previousPosition = transform.position;
    }

    void Update()
    {
        HandleInput();

        if (grounded && MoveCheck())
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * TurnStrength * Time.deltaTime, 0f));
        }

        Vector3 offsetSphereTransform = SphereRigidbody.transform.position - new Vector3(0, 0.45f, 0);
        transform.position = offsetSphereTransform;
    }

    private void FixedUpdate()
    {
        GroundCheck();
        MoveCheck();

    }

    private void HandleInput()
    {
        speedInput = 0f;
        if (Input.GetAxis("Vertical") > 0)
        {
            speedInput = Input.GetAxis("Vertical") * ForwardAcceleration * accelerationMultiplier;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            speedInput = Input.GetAxis("Vertical") * ReverseAcceleration * accelerationMultiplier;
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

            Vector3 forceDirection = Vector3.Cross(transform.right, hit.normal).normalized;

            if (Mathf.Abs(speedInput) > 0)
            {
                SphereRigidbody.AddForce(forceDirection * speedInput);
            }
        }
        else
        {
            SphereRigidbody.drag = 0.1f;
            SphereRigidbody.AddForce(Vector3.up * -GravityForce * forceMultiplier);
        }
    }

    private bool MoveCheck()
    {
        currentPosition = transform.position;

        if ((previousPosition - currentPosition).sqrMagnitude > 0.001f)
        {
            previousPosition = currentPosition;
            return true;
        }
        else
            return false;
    }
}
