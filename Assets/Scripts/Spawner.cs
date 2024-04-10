using System;
using Photon.Pun;
using Tuning;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private CarList carList;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject canvases;
    
    private void Awake()
    {
        Instantiate(canvases).GetComponent<Multiplayer>().Setup(this);
    }

    public void Spawn()
    {
        var spawnIndex = (PhotonNetwork.CurrentRoom.PlayerCount - 1) % spawnPoints.Length;
        
        var car = PhotonNetwork.Instantiate(carList[Player.Instance.SelectedCar].carPrefab.name,
            spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);

        car.GetComponent<CarTuning>().Data.ApplyTuning();
        car.GetComponent<CarSetup>().SetupLocalCar();
        
    }
}
