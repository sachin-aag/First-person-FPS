using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero;
    private Rigidbody rb;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotationx = 0f;
    private float currentCameraRotationx = 0f;
    private Vector3 thrusterForce = Vector3.zero;
    [SerializeField] private Camera cam;

    [SerializeField] private float cameraRotationLimit = 85f;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    //Get movement vector
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }
    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }
    void PerformMovement()
    {
        if(velocity != Vector3.zero)
        {
            //Moveposition checks for collision
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
        if (thrusterForce != Vector3.zero)
        {
            rb.AddForce(thrusterForce*Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }
    public void RotateCamera(float _cameraRotationX)
    {
        cameraRotationx = _cameraRotationX;
    }
    void PerformRotation()
    {
        if (rotation != Vector3.zero)
        {

            rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        }
        if( cam != null)
        {
            currentCameraRotationx -= cameraRotationx;
            currentCameraRotationx = Mathf.Clamp(currentCameraRotationx, -cameraRotationLimit, cameraRotationLimit);
            cam.transform.localEulerAngles = new Vector3(currentCameraRotationx, 0, 0);
        }
    }
    public void ApplyThruster(Vector3 _thrusterForce)
    {
        thrusterForce = _thrusterForce;
    }
}
