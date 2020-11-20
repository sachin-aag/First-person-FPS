using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero;
    private Rigidbody rb;
    private Vector3 rotation = Vector3.zero;
    private Vector3 cameraRotation = Vector3.zero;
    [SerializeField] private Camera cam;
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
    }
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }
    public void RotateCamera(Vector3 _cameraRotation)
    {
        cameraRotation = _cameraRotation;
    }
    void PerformRotation()
    {
        if (rotation != Vector3.zero)
        {

            rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        }
        if(cameraRotation != Vector3.zero && cam != null)
        {
            cam.transform.Rotate(cameraRotation);
        }
    }
}
