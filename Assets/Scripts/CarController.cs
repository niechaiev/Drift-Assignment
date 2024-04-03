using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] WheelCollider[] wheels;
    [SerializeField] private GameObject[] wheelMesh;
    [SerializeField] private GameObject centerOfMass;
    [SerializeField] private float torque;
    [SerializeField] private float steeringMax;
    [SerializeField] private float brakePower;
    

    private void Awake()
    {
        GetComponent<Rigidbody>().centerOfMass = centerOfMass.transform.localPosition;
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            wheels[2].motorTorque = wheels[3].motorTorque = torque;
        }
        else
        {
            for (var i = 0; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = 0;
            }
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            for (var i = 0; i < wheels.Length - 2; i++)
            {
                wheels[i].steerAngle = Input.GetAxis("Horizontal") * steeringMax;
            }
        }
        else
        {
            for (var i = 0; i < wheels.Length - 2; i++)
            {
                wheels[i].steerAngle = 0;
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            wheels[2].brakeTorque = wheels[3].brakeTorque = brakePower;
        }
        else
        {
            wheels[2].brakeTorque = wheels[3].brakeTorque = 0;
        }

        AnimateWheels();
    }

    private void AnimateWheels()
    {
        for (var i = 0; i < wheelMesh.Length; i++)
        {
            wheels[i].GetWorldPose(out var wheelPosition, out var wheelRotation);
            wheelMesh[i].transform.position = wheelPosition;
            wheelMesh[i].transform.rotation = wheelRotation;
        }
    }
}