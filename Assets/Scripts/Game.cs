using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Timer timer;
    [SerializeField] private Score score;
    [SerializeField] private Menu menu;
    [SerializeField] private Speedometer speedometer;
    
    private GameObject carGameObject;
    private Rigidbody carRigidBody;
    private CarController carController;
    private Action onTimeOut;

    private void Start()
    {
        carGameObject = GameObject.FindGameObjectWithTag("Player");
        carRigidBody = carGameObject.GetComponent<Rigidbody>();
        carController = carGameObject.GetComponent<CarController>();
        
        timer.StartTimer(20f);
        timer.OnTimeOut = Finish;
        score.Setup(carController);
        speedometer.Setup(carRigidBody);
    }

    private void Finish()
    {
        Player.Cash += score.Count;
        menu.Show();
        carController.enabled = false;
    }
}