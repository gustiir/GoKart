using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private Camera camera;

    [SerializeField]
    private float PositionSmoothSpeed = 10f;

    [SerializeField]
    private float RotationSmoothSpeed = 10f;

    private Vector3 desiredPosition;
    private Vector3 smoothedPosition;

    [SerializeField]
    private GoKartController goKartController;

    [SerializeField]
    public AnimationCurve fovCurve;

    [SerializeField]
    public AnimationCurve desiredCamPosYCurve;

    void LateUpdate()
    {
        float distanceFromCameraToTarget = (transform.position - target.transform.position).sqrMagnitude;
        camera.fieldOfView = 60f + fovCurve.Evaluate(distanceFromCameraToTarget);

        desiredPosition = target.transform.position;
        desiredPosition.y = 1.3f + desiredCamPosYCurve.Evaluate(distanceFromCameraToTarget);

        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, PositionSmoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;



        Quaternion desiredRotation = target.transform.rotation;
        Quaternion smoothedRotation = Quaternion.Lerp(transform.rotation, desiredRotation, RotationSmoothSpeed * Time.deltaTime);
        transform.rotation = smoothedRotation;

    }
}
