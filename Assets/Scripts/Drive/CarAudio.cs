using Photon.Pun;
using UnityEngine;

namespace Drive
{
    public class CarAudio : MonoBehaviourPun
    {
        [SerializeField] private AudioSource tiresAudioSource;
        [SerializeField] private AudioSource engineAudioSource;
        [SerializeField] private AudioClip idleAudioClip;
        [SerializeField] private AudioClip accelerationAudioClip;
        
        private const float SlipThreshold = 0.4f;
        private CarController _carController;

        private void Awake()
        {
            _carController = GetComponent<CarController>();
        }

        private void FixedUpdate()
        {
            PlayTireScreech();
            PlayEngineAudio();
        }

        private void PlayEngineAudio()
        {
            if (_carController.Wheels[2].motorTorque < 1)
            {
                if (engineAudioSource.clip != idleAudioClip)
                {
                    if(engineAudioSource.volume > 0.1f)
                    {
                        engineAudioSource.volume = Mathf.Lerp(engineAudioSource.volume, 0f, Time.deltaTime * 2);
                    }
                    else
                    {
                        engineAudioSource.volume = 1;
                        engineAudioSource.clip = idleAudioClip;
                        engineAudioSource.Play();
                    }
                }
                else
                {
                    engineAudioSource.pitch = 1 + Mathf.Clamp01(_carController.Kph / 50);
                }
            }
            else
            {
                engineAudioSource.volume = Mathf.Clamp01(0.4f + Mathf.Clamp01(_carController.Wheels[2].motorTorque / 800 * _carController.Kph / 100));
                engineAudioSource.pitch =
                    0.4f + Mathf.Clamp01(_carController.Wheels[2].motorTorque / 800 * _carController.Kph / 100);
                if (engineAudioSource.clip != accelerationAudioClip)
                {
                    engineAudioSource.clip = accelerationAudioClip;
                    engineAudioSource.Play();
                }
            }

            if (!PhotonNetwork.OfflineMode)
            {
                photonView.RPC(nameof(RPC_PlayEngineAudio), RpcTarget.Others,
                    engineAudioSource.clip == idleAudioClip,
                    engineAudioSource.volume,
                    engineAudioSource.pitch);
            }
        }
        
        [PunRPC]
        private void RPC_PlayEngineAudio(bool isIdleAudio, float volume, float pitch)
        {
            var clip = isIdleAudio ? idleAudioClip : accelerationAudioClip;
            
            if (engineAudioSource.clip != clip)
            {
                engineAudioSource.clip = clip;
                engineAudioSource.Play();
            }

            engineAudioSource.volume = volume;
            engineAudioSource.pitch = pitch;

        }

        private void PlayTireScreech()
        {
            var slips = 0f;
            for (var i = 2; i < 4; i++)
            {
                _carController.Wheels[i].GetGroundHit(out var wheelHit);
                slips += Mathf.Max(Mathf.Abs(wheelHit.sidewaysSlip), Mathf.Abs(wheelHit.forwardSlip));
            }

            slips /= 2;

            if (slips >= SlipThreshold)
            {
                var loudness = slips * Mathf.Clamp01(_carController.Kph / 40) * Random.Range(0.9f, 1.1f);
                tiresAudioSource.volume = loudness;
                tiresAudioSource.pitch = Mathf.Lerp(0.7f, 1.2f, loudness);
            }
            else
            {
                tiresAudioSource.volume = Mathf.Lerp(tiresAudioSource.volume, 0, Time.deltaTime * 8);
            }

            if (!PhotonNetwork.OfflineMode)
            {
                photonView.RPC(nameof(RPC_PlayTireScreech), RpcTarget.Others, tiresAudioSource.volume,
                    tiresAudioSource.pitch);
            }
        }
        
        [PunRPC]
        private void RPC_PlayTireScreech(float volume, float pitch)
        {
            tiresAudioSource.volume = volume;
            tiresAudioSource.pitch = pitch;
        }

    }
}
