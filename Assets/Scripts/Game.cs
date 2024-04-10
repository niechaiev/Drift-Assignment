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

    public void SetupGame()
    {
        carGameObject = GameObject.FindGameObjectWithTag("Player");
        carRigidBody = carGameObject.GetComponent<Rigidbody>();
        carController = carGameObject.GetComponent<CarController>();
        
        timer.OnTimeOut = Finish;
        score.Setup(carController);
        speedometer.Setup(carRigidBody);
    }

    public void StartGame()
    {
        timer.StartTimer(599);
    }

    private void Finish()
    {
        Player.Instance.Cash += score.Count;
        menu.Show();
        carController.enabled = false;
        score.enabled = false;
        timer.enabled = false;
    }
}