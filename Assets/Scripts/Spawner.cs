using Photon.Pun;
using Tuning;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private CarList carList;
    [SerializeField] private Transform[] spawnPoints;
    

    public void Spawn()
    {
        var car = PhotonNetwork.Instantiate(carList[Player.Instance.SelectedCar].carPrefab.name, spawnPoints[0].position, Quaternion.identity);
        car.GetComponent<CarTuning>().Data.ApplyTuning();
        car.GetComponent<CarSetup>().SetupLocalCar();
        
    }
}
