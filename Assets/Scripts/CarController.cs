using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] WheelCollider[] wheels;
    [SerializeField] private float torque;
    
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            for (var i = 0; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = torque;
            }
        }
        else
        {
            for (var i = 0; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = 0;
            }
        }
    }
}