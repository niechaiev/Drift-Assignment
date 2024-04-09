using Tuning;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private CarList carList;
    [SerializeField] private Transform[] spawnPoints;
    

    private void Awake()
    {
        var car = Instantiate(carList[Player.Instance.SelectedCar].carPrefab, spawnPoints[0]);
        car.GetComponent<CarTuning>().Data.ApplyTuning();
    }
}
