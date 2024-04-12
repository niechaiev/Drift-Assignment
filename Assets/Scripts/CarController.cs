using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    [SerializeField] WheelCollider[] wheels;
    [SerializeField] private GameObject[] wheelMeshes;
    [SerializeField] private GameObject centerOfMass;
    [SerializeField] private float torque;
    [SerializeField] private float steeringMax;
    [SerializeField] private float brakePower;
    private Rigidbody _playerRb; 
    private bool _isDrifting;
    private Joystick _joystick;
    private PressReleaseHandler _handbreakHandler;
    private PressReleaseHandler _forwardHandler;
    private PressReleaseHandler _reverseHandler;

    public bool IsDrifting => _isDrifting;

    public void Init(Joystick joystick, PressReleaseHandler handbreakHandler, PressReleaseHandler forwardHandler, PressReleaseHandler reverseHandler)
    {
        _joystick = joystick;
        _handbreakHandler = handbreakHandler;
        _forwardHandler = forwardHandler;
        _reverseHandler = reverseHandler;

        _handbreakHandler.OnPressed = () =>
        {
            var forwardFriction = wheels[2].forwardFriction;
            var sidewaysFriction = wheels[2].sidewaysFriction;
            wheels[2].brakeTorque = wheels[3].brakeTorque = brakePower;
            forwardFriction.stiffness = sidewaysFriction.stiffness = 0.33f;
            wheels[2].forwardFriction = wheels[3].forwardFriction = forwardFriction;
            wheels[2].sidewaysFriction = wheels[3].sidewaysFriction = sidewaysFriction;
        };

        _handbreakHandler.OnReleased = () =>
        {
            var forwardFriction = wheels[2].forwardFriction;
            var sidewaysFriction = wheels[2].sidewaysFriction;
            wheels[2].brakeTorque = wheels[3].brakeTorque = 0;
            forwardFriction.stiffness = sidewaysFriction.stiffness = 1f;
            wheels[2].forwardFriction = wheels[3].forwardFriction = forwardFriction;
            wheels[2].sidewaysFriction = wheels[3].sidewaysFriction = sidewaysFriction;
        };

        _forwardHandler.OnPressed = () =>
        {
            wheels[2].motorTorque = wheels[3].motorTorque = torque;
        };
        _forwardHandler.OnReleased = ReleasePedal;

        _reverseHandler.OnPressed = () =>
        {
            wheels[2].motorTorque = wheels[3].motorTorque = -torque;
        };
        _reverseHandler.OnReleased = ReleasePedal;

    }

    private void ReleasePedal()
    {
        foreach (var wheel in wheels)
        {
            wheel.motorTorque = 0;
        }
    }


    private void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
        _playerRb.centerOfMass = centerOfMass.transform.localPosition;
    }

    private void FixedUpdate()
    {
        CheckSteer();
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
                if (i == 3) _isDrifting = true;
                continue;
            }

            _isDrifting = false;
            break;
        }
    }

    private void CheckSteer()
    {
        if (_joystick.Horizontal != 0 || !_isDrifting)
        {
            var horizontal = _joystick.Horizontal;
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
            var angle = Vector3.SignedAngle(transform.forward, _playerRb.velocity, Vector3.up);
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
    

    private void OnDisable()
    {
        foreach (var wheel in wheels)
        {
            wheel.motorTorque = 0;
        }
    }
}