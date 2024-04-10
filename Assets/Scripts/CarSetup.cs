using UnityEngine;

public class CarSetup : MonoBehaviour
{

    public void SetupLocalCar()
    {
        GetComponent<CarController>().enabled = true;
    }
}
