using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speedMultiplier;
    private GameObject _player;
    private GameObject _cameraLocation;
    private float _speed;
    
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _cameraLocation = GameObject.FindGameObjectWithTag("CameraLocation");
    }
    
    private void FixedUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        _speed = Vector3.Distance(gameObject.transform.position, _cameraLocation.transform.position) * speedMultiplier;
        gameObject.transform.position =
            Vector3.Lerp(transform.position, _cameraLocation.transform.position, Time.deltaTime * _speed);
        gameObject.transform.LookAt(_player.transform.position);
    }
}
