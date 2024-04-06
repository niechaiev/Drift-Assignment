using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] WheelCollider[] wheels;
    [SerializeField] private GameObject[] wheelMeshes;
    [SerializeField] private GameObject centerOfMass;
    [SerializeField] private float torque;
    [SerializeField] private float steeringMax;
    [SerializeField] private float brakePower;
    private Rigidbody playerRb; 
    private bool isDrifting;

    public bool IsDrifting => isDrifting;


    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.centerOfMass = centerOfMass.transform.localPosition;
    }

    private void FixedUpdate()
    {
        CheckForward();
        CheckSteer();
        CheckBrake();
        
        AnimateWheels();

        CheckDrift();
    }



    private void CheckDrift()
    {
        for (var i = 0; i < 4; i++)
        {
            wheels[i].GetGroundHit(out var wheelHit);

            if (wheelHit.sidewaysSlip >= .3f || wheelHit.sidewaysSlip <= -.3f ||
                wheelHit.forwardSlip >= .3f || wheelHit.forwardSlip <= -.3f)
            {
                if (i == 3) isDrifting = true;
                continue;
            }

            isDrifting = false;
            break;
        }
    }

    private void CheckForward()
    {
        
        if (Input.GetKey(KeyCode.W))
        {
            wheels[2].motorTorque = wheels[3].motorTorque = torque;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            wheels[2].motorTorque = wheels[3].motorTorque = -torque;
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.motorTorque = 0;
            }
        }
    }

    private void CheckSteer()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || !isDrifting)
        {
            var horizontal = Input.GetAxis("Horizontal");
            if (horizontal != 0)
            {
                wheels[0].steerAngle = horizontal * steeringMax;
                wheels[1].steerAngle = horizontal * steeringMax;
            }
            else
            {
                wheels[0].steerAngle = Mathf.Lerp(wheels[0].steerAngle, 0, Time.deltaTime);
                wheels[1].steerAngle = Mathf.Lerp(wheels[1].steerAngle, 0, Time.deltaTime);
            }
        }
        else
        {
            var angle = Vector3.SignedAngle(transform.forward, playerRb.velocity, Vector3.up);
            wheels[0].steerAngle = Mathf.Lerp(wheels[0].steerAngle, Mathf.Clamp(angle, -steeringMax, steeringMax), Time.deltaTime);
            wheels[1].steerAngle = Mathf.Lerp(wheels[1].steerAngle, Mathf.Clamp(angle, -steeringMax, steeringMax), Time.deltaTime);
        }
    }

    private void AnimateWheels()
    {
        for (var i = 0; i < wheelMeshes.Length; i++)
        {
            wheels[i].GetWorldPose(out var wheelPosition, out var wheelRotation);
            wheelMeshes[i].transform.position = wheelPosition;
            wheelMeshes[i].transform.rotation = wheelRotation;
        }
    }

    private void CheckBrake()
    {
        var forwardFriction = wheels[2].forwardFriction;
        var sidewaysFriction = wheels[2].sidewaysFriction;

        if (Input.GetKey(KeyCode.Space))
        {
            wheels[2].brakeTorque = wheels[3].brakeTorque = brakePower;

            forwardFriction.stiffness = sidewaysFriction.stiffness = 0.33f;
            wheels[2].forwardFriction = wheels[3].forwardFriction = forwardFriction;
            wheels[2].sidewaysFriction = wheels[3].sidewaysFriction = sidewaysFriction;
        }
        else
        {
            wheels[2].brakeTorque = wheels[3].brakeTorque = 0;

            forwardFriction.stiffness = sidewaysFriction.stiffness = 1f;
            wheels[2].forwardFriction = wheels[3].forwardFriction = forwardFriction;
            wheels[2].sidewaysFriction = wheels[3].sidewaysFriction = sidewaysFriction;
        }
    }

    private void OnDisable()
    {
        foreach (var wheel in wheels)
        {
            wheel.motorTorque = 0;
        }
    }
}