using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float speed;
    [SerializeField] private float speedMultiplier;
    
    private GameObject player;
    private GameObject cameraLocation;
    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cameraLocation = GameObject.FindGameObjectWithTag("CameraLocation");
    }
    
    private void FixedUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        speed = Vector3.Distance(gameObject.transform.position, cameraLocation.transform.position) * speedMultiplier;
        gameObject.transform.position =
            Vector3.Lerp(transform.position, cameraLocation.transform.position, Time.deltaTime * speed);
        gameObject.transform.LookAt(player.transform.position);
    }
}
