using Level;
using Level.UI;
using Photon.Pun;
using UnityEngine;

namespace Drive
{
    public class CarController : MonoBehaviourPun
    {
        private const float MsToKphRatio = 3.6f;
    
        private const float MinDriftAngle = 20;
        private const float MaxDriftAngle = 160;
        private const float MinDriftSpeed = 20;
    
        private const float ThresholdAngularVelocity = 0.8f;
        private const float DriftingTorque = 1500f;
        
        private const float HandbrakeStiffness = 0.33f;

        [SerializeField] private WheelCollider[] wheels;
        [SerializeField] private GameObject[] wheelMeshes;
        [SerializeField] private GameObject centerOfMass;
        [SerializeField] private float torque;
        [SerializeField] private float maxSteering = 35f;
        [SerializeField] private float handbrakePower = 90000f;
        [SerializeField] private float brakePower = 20000f;

    
        private Rigidbody _carRigidbody; 
        private bool _isDrifting;
        private InputManager _inputManager;
        private float _kph;

        public float Kph => _kph;
        public float Torque => torque;


        public WheelCollider[] Wheels => wheels;

        public bool IsDrifting => _isDrifting;
    
        public void Init(InputManager inputManager)
        {
            _inputManager = inputManager;
        }
    
        private void Start()
        {
            _carRigidbody = GetComponent<Rigidbody>();
            _carRigidbody.centerOfMass = centerOfMass.transform.localPosition;
        }

        private void FixedUpdate()
        {
            _kph = _carRigidbody.velocity.magnitude * MsToKphRatio;
            Move();
            Steer();
            Handbrake();
            CheckDrift();
            //AdjustTraction();
            AnimateWheels();
        }
    
        private void Move()
        {
            if (_inputManager.IsForward)
            { 
                ApplyBrakesToAllWheels(0);
                if (wheels[2].motorTorque < torque || !_isDrifting)
                {
                    ApplyTorqueToRearWheels(torque);
                }
            }
            else if (_inputManager.IsReverse)
            {
                if (Vector3.Dot(_carRigidbody.velocity, transform.forward) > 1)
                {
                    ApplyBrakesToAllWheels(brakePower);
                    ApplyTorqueToRearWheels(0, 2);
                }
                else
                {
                    ApplyBrakesToAllWheels(0);
                    ApplyTorqueToRearWheels(-Mathf.Lerp(0, torque, 3 / Kph));
                }
            }
            else
            {
                ApplyBrakesToAllWheels(0);
                ApplyTorqueToRearWheels(0, 2);
            }
        }

        private void ApplyTorqueToRearWheels(float motorTorque, float gainSpeed = 4)
        {
            wheels[2].motorTorque = wheels[3].motorTorque =
                Mathf.Lerp(wheels[2].motorTorque, motorTorque, Time.deltaTime * gainSpeed);
        }

        private void ApplyBrakesToAllWheels(float power)
        {
            foreach (var wheel in wheels)
            {
                wheel.brakeTorque = power;
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
            var steer = _carRigidbody.angularVelocity.y switch
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
                ApplyTorqueToRearWheels(DriftingTorque);
            }
        }

        private void Handbrake()
        {
            if (_inputManager.IsHandbrake)
            {
                wheels[2].brakeTorque = wheels[3].brakeTorque = handbrakePower;
                SetRearWheelsStiffness(HandbrakeStiffness);
            }
            else
            {
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
            var driftValue = transform.InverseTransformVector(_carRigidbody.velocity); 
            var driftAngle = Mathf.Abs(Mathf.Atan2(driftValue.x, driftValue.z) * Mathf.Rad2Deg);
            _isDrifting = driftAngle is > MinDriftAngle and < MaxDriftAngle && _kph > MinDriftSpeed;
        }
    
    


        private static void SetExtremumAsymptoteValues(ref WheelFrictionCurve sidewaysFriction, ref WheelFrictionCurve forwardFriction, float value)
        {
            sidewaysFriction.extremumValue = sidewaysFriction.asymptoteValue =
                forwardFriction.extremumValue = forwardFriction.asymptoteValue = value;
        }
    
        private void ApplyFriction(WheelFrictionCurve sidewaysFriction, WheelFrictionCurve forwardFriction, int numberOfWheels)
        {
            for (var i = 0; i < numberOfWheels; i++)
            {
                wheels[i].sidewaysFriction = sidewaysFriction;
                wheels[i].forwardFriction = forwardFriction;
            }
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

            if (!PhotonNetwork.OfflineMode)
                photonView.RPC(nameof(RPC_AnimateWheels), RpcTarget.Others, wheelRotations);
        }

        [PunRPC]
        private void RPC_AnimateWheels(Quaternion[] wheelRotations)
        {
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
}