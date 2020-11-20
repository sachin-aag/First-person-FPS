using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
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
    }
}
