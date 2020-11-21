using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lookSensitivity = 3f;
    [SerializeField] private float cameraSensitivity = -3f;
    [SerializeField] private float thrusterForce = 1000f;
    private ConfigurableJoint cj;
    [Header("Spring Settings")]
    [SerializeField] private float jointSpring = 20f;
    [SerializeField] private float jointMaxForce = 40f;
    private PlayerMotor motor;
    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        cj = GetComponent<ConfigurableJoint>();
        SetJointSettings(jointSpring, jointMaxForce);

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
        float _cameraRotationX = _xRot * cameraSensitivity;

        //Apply rotation 
        motor.RotateCamera(-_cameraRotationX);

        //Calculate and apply thruster force
        Vector3 _thrusterForce = Vector3.zero;
        if (Input.GetButton("Jump"))
        {
            _thrusterForce = Vector3.up * thrusterForce;
            SetJointSettings(0f, 0f);
        }
        else
        {
            SetJointSettings(jointSpring, jointMaxForce);
        }
        motor.ApplyThruster(_thrusterForce);

    }
    private void SetJointSettings(float _jointSpring, float _jointMaxForce)
    {
        cj.yDrive = new JointDrive { positionSpring = _jointSpring, maximumForce = _jointMaxForce };
    }
}
