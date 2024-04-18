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

        private void SetButtonsInteractable()
        {
            if (Player.Instance.OwnedCars.Count <= 1)
            {
                leftButton.interactable = false;
                rightButton.interactable = false;
            }
            else
            {
                leftButton.interactable = true;
                rightButton.interactable = true;
            }
        }
        
        public void SetModeSelectCar()
        {
            rightButton.onClick.RemoveListener(PreviewNextCar);
            leftButton.onClick.RemoveListener(PreviewPreviousCar);
            rightButton.onClick.AddListener(() => ChangeCar(Player.Instance.SelectNextCar()));
            leftButton.onClick.AddListener(() => ChangeCar(Player.Instance.SelectPreviousCar()));
            ChangeCar(Player.Instance.SelectedCar);
            _previewedCar = Player.Instance.SelectedCar;
            SetButtonsInteractable();
        }

        public void SetModePreviewCar()
        {
            rightButton.onClick.RemoveAllListeners();
            leftButton.onClick.RemoveAllListeners();
            rightButton.onClick.AddListener(PreviewNextCar);
            leftButton.onClick.AddListener(PreviewPreviousCar);
            leftButton.interactable = true;
            rightButton.interactable = true;
        }
        

        private void PreviewNextCar()
        {
            if (_previewedCar == carList.Count - 1)
                _previewedCar = 0;
            else
                _previewedCar++;
        
            ChangeCar(_previewedCar);
        }

        private void PreviewPreviousCar()
        {
            if (_previewedCar == 0)
                _previewedCar = carList.Count - 1;
            else
                _previewedCar--;

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
