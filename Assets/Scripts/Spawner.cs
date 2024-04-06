using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cars cars;
    [SerializeField] private Transform[] spawnPoints;
    

    private void Awake()
    {
        Instantiate(cars.carPrefabs[Player.SelectedCar], spawnPoints[0]);
    }
}