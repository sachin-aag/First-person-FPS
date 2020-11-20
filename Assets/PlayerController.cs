using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lookSensitivity = 3f;
    [SerializeField] private float cameraSensitivity = -3f;

    private PlayerMotor motor;
    private void Start()
    {
        motor = GetComponent<PlayerMotor>();

        
    }
    private void Update()
    {
        //Get velocity as 3d vector
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * _xMov; //(1,0,0)
        Vector3 moveVertical = transform.forward * _zMov;//(0,0,1)


        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

        motor.Move(velocity);
        float _yRot = Input.GetAxisRaw("Mouse X");
         Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

        //Apply rotation 
        motor.Rotate(_rotation);

        //Camera rotation
        float _xRot = Input.GetAxisRaw("Mouse Y");
        Vector3 _cameraRotation = new Vector3(_xRot, 0f, 0f) * cameraSensitivity;

        //Apply rotation 
        motor.RotateCamera(_cameraRotation);
    }
}
