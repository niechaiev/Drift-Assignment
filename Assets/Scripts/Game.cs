using System;
using Score;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Timer timer;
    [SerializeField] private ScoreInstantiator scoreInstantiator;
    [SerializeField] private Menu menu;
    [SerializeField] private Speedometer speedometer;
    [SerializeField] private Canvas steeringCanvas;
    [SerializeField] private InputManager inputManager;

    private Score.Score _currentScore;
    private GameObject _carGameObject;
    private CarController _carController;
    private Action _onTimeOut;

    public void SetupGame()
    {
        _carGameObject = GameObject.FindGameObjectWithTag("Player");
        _carController = _carGameObject.GetComponent<CarController>();
        _carController.Init(inputManager);
        timer.OnTimeOut = FinishGame;
        _currentScore = scoreInstantiator.InitializeScore(Player.Instance.IsOnline);
        _currentScore.Setup(_carController);
        speedometer.Setup(_carController);
    }

    public void StartGame()
    {
        timer.StartTimer(119);
        steeringCanvas.gameObject.SetActive(true);
    }

    private void FinishGame()
    {
        Player.Instance.Cash += _currentScore.Count;
        menu.Show();
        menu.ShowReward(_currentScore.Count);
        _carController.enabled = false;
        _currentScore.enabled = false;
        timer.enabled = false;
        steeringCanvas.gameObject.SetActive(false);
    }
    
}