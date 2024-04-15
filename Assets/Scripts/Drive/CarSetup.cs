using UnityEngine;

namespace Drive
{
    public class CarSetup : MonoBehaviour
    {

        public void SetupLocalCar()
        {
            GetComponent<CarController>().enabled = true;
            GetComponent<CarAudio>().enabled = true;
        }
    }
}
