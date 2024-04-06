using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarSelect : MonoBehaviour
{
    [SerializeField] private Transform spawn;
    [SerializeField] private Cars cars;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private TMP_Text nameText;
    private GameObject car;

    private void Awake()
    {
        rightButton.onClick.AddListener(NextCar);
        leftButton.onClick.AddListener(PreviousCar);
    }

    private void Start()
    {
        ChangeCar();
    }

    private void NextCar()
    {
        if (Player.SelectedCar == cars.carPrefabs.Length - 1)
        {
            Player.SelectedCar = 0;
        }
        else
        {
            Player.SelectedCar += 1;
        }
        
        ChangeCar();
    }

    private void PreviousCar()
    {
        if (Player.SelectedCar == 0)
        {
            Player.SelectedCar = cars.carPrefabs.Length - 1;
        }
        else
        {
            Player.SelectedCar -= 1;
        }

        ChangeCar();
    }

    private void ChangeCar()
    {
        if (car != null)
            Destroy(car);
        car = Instantiate(cars.carPrefabs[Player.SelectedCar], spawn);
        nameText.SetText(car.gameObject.name);

    }
}
