using UnityEngine;
using UnityEngine.UI;

namespace Garage
{
    public class TuningPage : Page
    {
        [SerializeField] private Button spoilerTuningButton;
        [SerializeField] private Button wheelTuningButton;
        [SerializeField] private Button colorTuningButton;
        private CarInfo selectedCarInfo;

        public void Setup(CarTuning carTuning)
        {
            gameObject.SetActive(true);
            spoilerTuningButton.gameObject.SetActive(carTuning.HasTuning(carTuning.SpoilerTuning));
            wheelTuningButton.gameObject.SetActive(carTuning.HasTuning(carTuning.WheelTuning));
            colorTuningButton.gameObject.SetActive(carTuning.HasTuning(carTuning.ColorTuning));
        }
    }
}