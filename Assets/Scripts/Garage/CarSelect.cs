using System.Linq;
using TMPro;
using Tuning;
using UnityEngine;
using UnityEngine.UI;

namespace Garage
{
    public class CarSelect : MonoBehaviour
    {
        [SerializeField] private Transform spawn;
        [SerializeField] private CarList carList;
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private CarShopPage carShopPage;
        
        private int _previewedCar;
        private GameObject _carInstance;

        public GameObject CarInstance => _carInstance;

        private Car _car;
        
        
        
        public void SetModeSelectCar()
        {
            rightButton.onClick.RemoveListener(PreviewNextCar);
            leftButton.onClick.RemoveListener(PreviewPreviousCar);
            rightButton.onClick.AddListener(SelectNextCar);
            leftButton.onClick.AddListener(SelectPreviousCar);
            ChangeCar(Player.Instance.SelectedCar);
            _previewedCar = Player.Instance.SelectedCar;
        }

        public void SetModePreviewCar()
        {
            rightButton.onClick.RemoveListener(SelectNextCar);
            leftButton.onClick.RemoveListener(SelectPreviousCar);
            rightButton.onClick.AddListener(PreviewNextCar);
            leftButton.onClick.AddListener(PreviewPreviousCar);
        
        }
    
        private void SelectNextCar()
        {
            if (Player.Instance.SelectedCar == Player.Instance.OwnedCars.Count - 1)
            {
                Player.Instance.SelectedCar = Player.Instance.OwnedCars[0];
            }
            else
            {
                var index = Player.Instance.OwnedCars.First(i => i == Player.Instance.SelectedCar);
                Player.Instance.SelectedCar = Player.Instance.OwnedCars[++index];
            }
        
            ChangeCar(Player.Instance.SelectedCar);
        }
    
        private void SelectPreviousCar()
        {
            if (Player.Instance.SelectedCar == Player.Instance.OwnedCars[0])
            {
                Player.Instance.SelectedCar = Player.Instance.OwnedCars.Count - 1;
            }
            else
            {
                var index = Player.Instance.OwnedCars.First(i => i == Player.Instance.SelectedCar);
                Player.Instance.SelectedCar = Player.Instance.OwnedCars[--index];
            }

            ChangeCar(Player.Instance.SelectedCar);
        }


        private void PreviewNextCar()
        {
            if (_previewedCar == carList.Count - 1)
            {
                _previewedCar = 0;
            }
            else
            {
                _previewedCar += 1;
            }
        
            ChangeCar(_previewedCar);
        }

        private void PreviewPreviousCar()
        {
            if (_previewedCar == 0)
            {
                _previewedCar = carList.Count - 1;
            }
            else
            {
                _previewedCar -= 1;
            }

            ChangeCar(_previewedCar);
        }

        private void ChangeCar(int selectedCarIndex)
        {
            if (_carInstance != null)
                Destroy(_carInstance);
            _car = carList[selectedCarIndex];
            _carInstance = Instantiate(_car.carPrefab, spawn);
            nameText.SetText(_car.carInfo.CarName);
            carShopPage.SetPreviewedCarInfo(_car.carInfo);

            _carInstance.GetComponent<CarTuning>().Data.ApplyTuning();
        }

        private void OnDisable()
        {
            rightButton.onClick.RemoveAllListeners();
            leftButton.onClick.RemoveAllListeners();
        }
    }
}
