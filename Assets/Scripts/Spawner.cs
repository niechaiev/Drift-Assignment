using Photon.Pun;
using Tuning;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private CarList carList;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject canvases;
    private GameObject _car;
    
    private void Awake()
    {
        var newCanvases = Instantiate(canvases);
        if (Player.Instance.IsOnline)
            newCanvases.GetComponent<Multiplayer>().Setup(this);
        else 
            newCanvases.GetComponent<Singleplayer>().Setup(this);
    }
    

    public void Spawn()
    {
        if (Player.Instance.IsOnline)
        {
            var spawnIndex = (PhotonNetwork.CurrentRoom.PlayerCount - 1) % spawnPoints.Length;

            _car = PhotonNetwork.Instantiate(carList[Player.Instance.SelectedCar].carPrefab.name,
                spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
        }
        else
        {
            _car = Instantiate(carList[Player.Instance.SelectedCar].carPrefab,
                spawnPoints[0].position, spawnPoints[0].rotation);
        }

        _car.GetComponent<CarTuning>().Data.ApplyTuning();
    }

    public void StartCar()
    {
        _car.GetComponent<CarSetup>().SetupLocalCar();
    }
}
