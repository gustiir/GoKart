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

    void LateUpdate()
    {
        Vector3 desiredPosition = target.transform.position;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, PositionSmoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        Quaternion desiredRotation = target.transform.rotation;
        Quaternion smoothedRotation = Quaternion.Lerp(transform.rotation, desiredRotation, RotationSmoothSpeed * Time.deltaTime);
        transform.rotation = smoothedRotation;
        
        float distanceFromCameraToTarget = (transform.position - target.transform.position).sqrMagnitude;
        camera.fieldOfView = 60f + distanceFromCameraToTarget;
    }
}
