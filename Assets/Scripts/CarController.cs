using Photon.Pun;
using UnityEngine;

public class CarController : MonoBehaviourPun
{
    private const float MinDriftAngle = 20;
    private const float MaxDriftAngle = 160;
    private const float MinDriftSpeed = 20;
    private const float ThresholdAngularVelocity = 0.8f;
    private readonly float _driftingTorque = 1500f;
    private readonly float _handBrakeFrictionMultiplier = 1.7f;
    
    [SerializeField] private WheelCollider[] wheels;
    [SerializeField] private GameObject[] wheelMeshes;
    [SerializeField] private GameObject centerOfMass;
    [SerializeField] private float torque;
    [SerializeField] private float maxSteering = 35f;
    [SerializeField] private float brakePower;
    
    private Rigidbody _playerRb; 
    private bool _isDrifting;
    private InputManager _inputManager;


    public WheelCollider[] Wheels => wheels;

    public bool IsDrifting => _isDrifting;
    
    public void Init(InputManager inputManager)
    {
        _inputManager = inputManager;
    }
    
    private void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
        _playerRb.centerOfMass = centerOfMass.transform.localPosition;
    }

    private void FixedUpdate()
    {
        Move();
        Steer();
        Handbrake();
        CheckDrift();
        AnimateWheels();
    }
    
    private void Move()
    {
        if (_inputManager.IsForward)
        {
            wheels[2].motorTorque = wheels[3].motorTorque = torque;
        }
        else if (_inputManager.IsReverse)
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

    private void Steer()
    {
        if (_inputManager.IsSteering || !_isDrifting)
        {
            var horizontal = _inputManager.HorizontalAxis;
            if (horizontal != 0)
            {
                wheels[0].steerAngle = wheels[1].steerAngle =
                    Mathf.Lerp(wheels[0].steerAngle, maxSteering * horizontal, Time.deltaTime * 4);
            }
            else
            {
                wheels[0].steerAngle = wheels[1].steerAngle = Mathf.Lerp(wheels[0].steerAngle, 0, Time.deltaTime * 4);
            }
        }
        else
        {
            AssistSteering();
        }
    }
    
    private void AssistSteering()
    {
        var steer = _playerRb.angularVelocity.y switch
        {
            > ThresholdAngularVelocity => -1,
            < -ThresholdAngularVelocity => 1,
            _ => 0f
        };

        wheels[0].steerAngle = wheels[1].steerAngle = Mathf.Lerp(wheels[0].steerAngle,
            Mathf.Clamp(steer * maxSteering, -maxSteering, maxSteering),
            Time.deltaTime * 4);

        if (_inputManager.IsForward)
        {
            wheels[2].motorTorque = wheels[3].motorTorque = _driftingTorque;
        }
    }

    private void Handbrake()
    {
        if (_inputManager.IsHandbrake)
        {
            wheels[2].brakeTorque = wheels[3].brakeTorque = brakePower;
            SetRearWheelsStiffness(0.33f);
        }
        else
        {
            wheels[2].brakeTorque = wheels[3].brakeTorque = 0;
            SetRearWheelsStiffness(1f);
        }
    }
    
    private void SetRearWheelsStiffness(float stiffness)
    {
        var forwardFriction = wheels[2].forwardFriction;
        var sidewaysFriction = wheels[2].sidewaysFriction;
        forwardFriction.stiffness = sidewaysFriction.stiffness = stiffness;
        wheels[2].forwardFriction = wheels[3].forwardFriction = forwardFriction;
        wheels[2].sidewaysFriction = wheels[3].sidewaysFriction = sidewaysFriction;
    }

    private void CheckDrift()
    {
        var driftValue = transform.InverseTransformVector(_playerRb.velocity); 
        var driftAngle = Mathf.Abs(Mathf.Atan2(driftValue.x, driftValue.z) * Mathf.Rad2Deg);
        _isDrifting = driftAngle is > MinDriftAngle and < MaxDriftAngle && _playerRb.velocity.magnitude * Speedometer.MsToKphRatio > MinDriftSpeed;
    }


    private void AnimateWheels()
    {
        var wheelRotations = new Quaternion[4];
        for (var i = 0; i < wheelMeshes.Length; i++)
        {
            wheels[i].GetWorldPose(out _, out var wheelRotation);
            wheelMeshes[i].transform.rotation = wheelRotation;
            wheelRotations[i] = wheelRotation;
        }

        photonView.RPC("RPC_AnimateWheels", RpcTarget.Others, wheelRotations);
    }

    [PunRPC]
    private void RPC_AnimateWheels(Quaternion[] wheelRotations)
    {
        Debug.Log("RPC_AnimateWheels");
        for (var i = 0; i < 4; i++)
        {
            wheelMeshes[i].transform.rotation = wheelRotations[i];
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