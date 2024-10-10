using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float damping; 

    public Transform target;

    private Vector3 vel = Vector3.zero;
    private Vector3 lastKnownPosition;
    private void FixedUpdate()
    {
        //if(target != null) { 
        //Vector3 targetPosition = target.position + offset;
        //targetPosition.z = transform.position.z;

        //transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref
        //    vel, damping);


        if (target != null)
        {
            lastKnownPosition = target.position;
        }
        Vector3 targetPosition = (target != null && target.gameObject != null ? target.position : lastKnownPosition) + offset;

        //Vector3 targetPosition = (target?.position ?? lastKnownPosition) + offset;


        targetPosition.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref vel, damping);


        // Debug camera pos;
        Debug.LogFormat("Camera Position: {0}", transform.position.ToString("F3"));
    }
}
