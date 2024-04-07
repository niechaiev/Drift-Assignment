using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private CarList carList;
    [SerializeField] private Transform[] spawnPoints;
    

    private void Awake()
    {
        Instantiate(carList[Player.SelectedCar].carPrefab, spawnPoints[0]);
    }
}
