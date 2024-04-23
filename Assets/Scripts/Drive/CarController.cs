using System;
using Level.UI;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Drive
{
    public class CarController : MonoBehaviourPun
    {
        [SerializeField] private float maxSteerAngle = 35f;
        [SerializeField] private float thresholdAngularVelocity = 0.8f;
      
        [SerializeField] private float handbrakeStiffness = 0.33f;
        
        [SerializeField] private float torque = 800f;
        [SerializeField] private float driftingTorque = 1500f;
        [SerializeField] private float handbrakeTorque = 90000f;
        [SerializeField] private float brakeTorque = 20000f;
        
        [SerializeField] private float smoothFactor = 0.7f;
        [SerializeField] private float frontWheelGrip = 1.1f;
        [SerializeField] private float allWheelGripFactor = 1.7f;
        
        [SerializeField] private WheelCollider[] wheels;
        [SerializeField] private GameObject[] wheelMeshes;
        [SerializeField] private GameObject centerOfMass;
        
        private Rigidbody _carRigidbody; 
        private bool _isDrifting;
        private InputManager _inputManager;
        private float _kph;
        private TMP_Text _debugText;

        public float Kph => _kph;
        public float Torque => torque;
        public WheelCollider[] Wheels => wheels;
        public GameObject[] WheelMeshes => wheelMeshes;
        public bool IsDrifting => _isDrifting;
    
        public void Init(InputManager inputManager)
        {
            _inputManager = inputManager;
        }
    
        private void Start()
        {
            _carRigidbody = GetComponent<Rigidbody>();
            _carRigidbody.centerOfMass = centerOfMass.transform.localPosition;
            _debugText = GameObject.FindGameObjectWithTag("Debug").GetComponent<TMP_Text>();
        }

        private void FixedUpdate()
        {
            const float msToKphRatio = 3.6f;
            _kph = _carRigidbody.velocity.magnitude * msToKphRatio;
            Move();
            Steer();
            CheckDrift();
            AdjustTraction();
            Handbrake();
            AnimateWheels();
        }
    
        private void Move()
        {
            ApplyBrakesToAllWheels(0);
            if (_inputManager.IsForward)
            { 
                if (wheels[2].motorTorque < torque || !_isDrifting)
                {
                    ApplyTorqueToRearWheels(torque);
                }
            }
            else if (_inputManager.IsReverse)
            {
                if (Vector3.Dot(_carRigidbody.velocity, transform.forward) > 1)
                {
                    ApplyBrakesToAllWheels(brakeTorque);
                    ApplyTorqueToRearWheels(0, 2);
                }
                else
                {
                    ApplyTorqueToRearWheels(-Mathf.Lerp(0, torque, 3 / Kph));
                }
            }
            else
            {
                ApplyTorqueToRearWheels(0, 2);
            }
        }

        private void ApplyTorqueToRearWheels(float motorTorque, float gainSpeed = 4)
        {
            if (Math.Abs(wheels[2].motorTorque - motorTorque) < 1f)
            {
                wheels[2].motorTorque = wheels[3].motorTorque = motorTorque;
            }
            else
            {
                wheels[2].motorTorque = wheels[3].motorTorque =
                    Mathf.Lerp(wheels[2].motorTorque, motorTorque, Time.deltaTime * gainSpeed);
            }
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
                        Mathf.Lerp(wheels[0].steerAngle, maxSteerAngle * horizontal, Time.deltaTime * 4);
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
            float steer;
            if (_carRigidbody.angularVelocity.y > thresholdAngularVelocity)
                steer = -1;
            else if (_carRigidbody.angularVelocity.y < -thresholdAngularVelocity)
                steer = 1;
            else
                steer = 0f;

            wheels[0].steerAngle = wheels[1].steerAngle = Mathf.Lerp(wheels[0].steerAngle,
                Mathf.Clamp(steer * maxSteerAngle, -maxSteerAngle, maxSteerAngle),
                Time.deltaTime * 4);

            if (_inputManager.IsForward)
            {
                ApplyTorqueToRearWheels(driftingTorque);
            }
        }

        private void Handbrake()
        {
            if (_inputManager.IsHandbrake)
            {
                wheels[2].brakeTorque = wheels[3].brakeTorque = handbrakeTorque;
                SetRearWheelsStiffness(handbrakeStiffness);
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
            const float minDriftAngle = 20;
            const float maxDriftAngle = 160;
            const float minDriftSpeed = 20;
            
            var driftValue = transform.InverseTransformVector(_carRigidbody.velocity); 
            var driftAngle = Mathf.Abs(Mathf.Atan2(driftValue.x, driftValue.z) * Mathf.Rad2Deg);
            _isDrifting = driftAngle is > minDriftAngle and < maxDriftAngle && _kph > minDriftSpeed;
        }

        private void AdjustTraction()
        {
            var driftAmount = 0f;
            
            for (var i = 2; i < 4; i++)
            {
                wheels[i].GetGroundHit(out var wheelHit);

                var steerFraction = wheels[0].steerAngle / maxSteerAngle;
                
                if (wheelHit.sidewaysSlip < 0)
                    driftAmount = (1 - steerFraction) * Mathf.Abs(wheelHit.sidewaysSlip);
                else if (wheelHit.sidewaysSlip > 0)
                    driftAmount = (1 + steerFraction) * Mathf.Abs(wheelHit.sidewaysSlip);
                
                _debugText.SetText($"sidewayslip: {wheelHit.sidewaysSlip}\n" +
                                   $"steerFraction: {steerFraction}\n" +
                                   $"driftAmount: {driftAmount}");
            }
            
            var sidewaysFriction = wheels[0].sidewaysFriction;
            var forwardFriction = wheels[0].forwardFriction;
            if (_inputManager.IsHandbrake)
            {
                var velocity = 0f;
                var smoothTime = smoothFactor * Time.deltaTime;
                var allWheelGrip = Mathf.SmoothDamp(forwardFriction.asymptoteValue, driftAmount * allWheelGripFactor,
                    ref velocity, smoothTime);
                
                SetExtremumAsymptoteValues(ref sidewaysFriction, ref forwardFriction, allWheelGrip);
                ApplyFriction(sidewaysFriction, forwardFriction, 2, 4);

                SetExtremumAsymptoteValues(ref sidewaysFriction, ref forwardFriction, frontWheelGrip);
                ApplyFriction(sidewaysFriction, forwardFriction, 0, 2);
            }
            else
            {
                const float topSpeed = 300f;
                const float minGrip = 1f;
                var allWheelGrip = allWheelGripFactor * _kph / topSpeed + minGrip;
                
                SetExtremumAsymptoteValues(ref sidewaysFriction, ref forwardFriction, allWheelGrip);
                ApplyFriction(sidewaysFriction, forwardFriction, 0, 4);
            }
        }


        private void SetExtremumAsymptoteValues(ref WheelFrictionCurve sidewaysFriction, ref WheelFrictionCurve forwardFriction, float value)
        {
            sidewaysFriction.extremumValue = sidewaysFriction.asymptoteValue = value;
            forwardFriction.extremumValue = forwardFriction.asymptoteValue = value;
        }
    
        private void ApplyFriction(WheelFrictionCurve sidewaysFriction, WheelFrictionCurve forwardFriction, int firstWheel, int lastWheel)
        {
            for (var i = firstWheel; i < lastWheel; i++)
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