using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lookSensitivity = 3f;
    [SerializeField] private float cameraSensitivity = -3f;
    [SerializeField] private float thrusterForce = 1000f;
    [SerializeField] private float thrusterFuelBurnSpeed = 1f;

    [SerializeField] private float thrusterFuelRegenSpeed = 0.3f;
    private float thrusterFuelAmount = 1f;
    private ConfigurableJoint cj;
    [Header("Spring Settings")]
    [SerializeField] private float jointSpring = 20f;
    [SerializeField] private float jointMaxForce = 40f;
    private Animator animator;
    private PlayerMotor motor;
    [SerializeField]
    private LayerMask environmentMask;
    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        cj = GetComponent<ConfigurableJoint>();
        SetJointSettings(jointSpring, jointMaxForce);
        animator = GetComponent<Animator>();

    }
    private void Update()
    {
        //Setting target position for spring. 
        //this makes the phyiscis right when applying gravity to flying sht
        RaycastHit _hit;
        if(Physics.Raycast(transform.position, Vector3.down, out _hit, 100f, environmentMask))
        {
            cj.targetPosition = new Vector3(0f, -_hit.point.y, 1f);
        }
        else
        {
            cj.targetPosition = new Vector3(0f, 0f, 1f);
        }
        //Get velocity as 3d vector
        float _xMov = Input.GetAxis("Horizontal");
        float _zMov = Input.GetAxis("Vertical");

        Vector3 moveHorizontal = transform.right * _xMov; //(1,0,0)
        Vector3 moveVertical = transform.forward * _zMov;//(0,0,1)


        Vector3 velocity = (moveHorizontal + moveVertical)* speed;
        animator.SetFloat("ForwardVelocity", _zMov);

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
        if (Input.GetButton("Jump") && thrusterFuelAmount > 0)
        {
            thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;
            if(thrusterFuelAmount >= 0.01f)
            {
                _thrusterForce = Vector3.up * thrusterForce;
                SetJointSettings(0f, 0f);
            }
            

        }
        else 
        {
            thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;
            
            SetJointSettings(jointSpring, jointMaxForce);
        }
        thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0f, 1f);
        motor.ApplyThruster(_thrusterForce);

    }
    public float GetThrusterFuelAmount()
    {
        return thrusterFuelAmount;
    }
    private void SetJointSettings(float _jointSpring, float _jointMaxForce)
    {
        cj.yDrive = new JointDrive { positionSpring = _jointSpring, maximumForce = _jointMaxForce };
    }
}
