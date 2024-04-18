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
                    const float minEngineVolume = 0.4f;
                    
                    if (engineAudioSource.volume > minEngineVolume)
                    {
                        engineAudioSource.volume = Mathf.Lerp(engineAudioSource.volume, 0f, Time.deltaTime * 2);
                    }
                    else
                    {
                        const float engineIdleVolume = 0.4f;
                        engineAudioSource.volume = engineIdleVolume;
                        engineAudioSource.clip = idleAudioClip;
                        engineAudioSource.Play();
                    }
                }
                else
                {
                    const int maxKphForIdle = 50;
                    
                    engineAudioSource.pitch = 1 + Mathf.Clamp01(_carController.Kph / maxKphForIdle);
                }
            }
            else
            {
                const int maxKph = 120;
                const float minVolumePitch = 0.6f;
                
                var volumePitch = Mathf.Clamp01(_carController.Wheels[2].motorTorque / _carController.Torque *
                    _carController.Kph / maxKph);

                engineAudioSource.volume = Mathf.Clamp01(minVolumePitch + volumePitch);
                engineAudioSource.pitch = minVolumePitch + volumePitch;
                
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
            var averageSlip = 0f;
            for (var i = 2; i < 4; i++)
            {
                _carController.Wheels[i].GetGroundHit(out var wheelHit);
                //Debug.Log("SIDE: " + Mathf.Abs(wheelHit.sidewaysSlip));
                //Debug.Log("____: " + Mathf.Abs(wheelHit.forwardSlip));

                averageSlip += (Mathf.Abs(wheelHit.sidewaysSlip) + Mathf.Abs(wheelHit.forwardSlip)) / 2;
            }

            averageSlip /= 2;
            
            const float slipThreshold = 0.4f;
            const int maxKphForLoudness = 40;
            
            const float minRandom = 0.9f;
            const float maxRandom = 1.1f;
            
            const float minPitch = 0.7f;
            const float maxPitch = 1.2f;
            
            if (averageSlip >= slipThreshold)
            {
                var loudness = averageSlip * Mathf.Clamp01(_carController.Kph / maxKphForLoudness) *
                               Random.Range(minRandom, maxRandom);
                tiresAudioSource.volume = loudness;
                tiresAudioSource.pitch = Mathf.Lerp(minPitch, maxPitch, loudness);
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
