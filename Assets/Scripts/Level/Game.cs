﻿using System;
using Drive;
using Level.UI;
using Photon.Pun;
using Score;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Level
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private Timer timer;
        [SerializeField] private ScoreInstantiator scoreInstantiator;
        [SerializeField] private PauseMenu menu;
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
            _currentScore = scoreInstantiator.InitializeScore(!PhotonNetwork.OfflineMode);
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
        
            var levelName = SceneManager.GetActiveScene().name;
            GAManager.OnMoneyGain(false, _currentScore.Count, "levelCompeted", levelName);
            GAManager.OnLevelComplete(levelName);
        
            menu.Show();
            menu.ShowReward(_currentScore.Count);
            _carController.enabled = false;
            _currentScore.enabled = false;
            timer.enabled = false;
            steeringCanvas.gameObject.SetActive(false);
        }
    
    }
}