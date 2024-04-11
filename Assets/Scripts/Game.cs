using System;
using Score;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Timer timer;
    [SerializeField] private ScoreInstantiator scoreInstantiator;
    [SerializeField] private Menu menu;
    [SerializeField] private Speedometer speedometer;

    private Score.Score _currentScore;
    private GameObject _carGameObject;
    private Rigidbody _carRigidBody;
    private CarController _carController;
    private Action _onTimeOut;

    public void SetupGame()
    {
        _carGameObject = GameObject.FindGameObjectWithTag("Player");
        _carRigidBody = _carGameObject.GetComponent<Rigidbody>();
        _carController = _carGameObject.GetComponent<CarController>();
        timer.OnTimeOut = Finish;
        _currentScore = scoreInstantiator.InitializeScore(Player.Instance.IsOnline);
        _currentScore.Setup(_carController);
        speedometer.Setup(_carRigidBody);
    }

    public void StartGame()
    {
        timer.StartTimer(9);
    }

    private void Finish()
    {
        Player.Instance.Cash += _currentScore.Count;
        menu.Show();
        menu.ShowReward(_currentScore.Count);
        _carController.enabled = false;
        _currentScore.enabled = false;
        timer.enabled = false;
    }
    
}