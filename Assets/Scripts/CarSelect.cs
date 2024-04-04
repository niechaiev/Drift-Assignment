using UnityEngine;

public class CarSelect : MonoBehaviour
{
    [SerializeField] private Transform spawn;
    [SerializeField] private Cars cars;

    private void Start()
    {
        Instantiate(cars.carPrefabs[Player.SelectedCar], spawn);
    }
}
