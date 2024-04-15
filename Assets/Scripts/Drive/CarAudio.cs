using UnityEngine;

namespace Drive
{
    public class CarAudio : MonoBehaviour
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
                        engineAudioSource.volume = Mathf.Lerp(engineAudioSource.volume, 0, Time.deltaTime * 2);
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
                return;
            }

            engineAudioSource.volume = Mathf.Clamp01(0.4f + Mathf.Clamp01(_carController.Wheels[2].motorTorque / 800 * _carController.Kph / 100));
            engineAudioSource.pitch =
                0.4f + Mathf.Clamp01(_carController.Wheels[2].motorTorque / 800 * _carController.Kph / 100);
            if (engineAudioSource.clip != accelerationAudioClip)
            {
                engineAudioSource.clip = accelerationAudioClip;
                engineAudioSource.Play();
            }
            
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
                    if (!tiresAudioSource.isPlaying)
                    {
                        tiresAudioSource.Play();
                    }
                }
                else
                {
                    tiresAudioSource.volume = Mathf.Lerp(tiresAudioSource.volume, 0, Time.deltaTime * 8);
                }
            }
    }
}
