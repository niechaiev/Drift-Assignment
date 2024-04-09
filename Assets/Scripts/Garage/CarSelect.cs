using System;
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
        
        private int previewedCar;
        private GameObject carInstance;

        public GameObject CarInstance => carInstance;

        private Car car;
        
        
        
        public void SetModeSelectCar()
        {
            rightButton.onClick.RemoveListener(PreviewNextCar);
            leftButton.onClick.RemoveListener(PreviewPreviousCar);
            rightButton.onClick.AddListener(SelectNextCar);
            leftButton.onClick.AddListener(SelectPreviousCar);
            ChangeCar(Player.Instance.SelectedCar);
            previewedCar = Player.Instance.SelectedCar;
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
            if (previewedCar == carList.Count - 1)
            {
                previewedCar = 0;
            }
            else
            {
                previewedCar += 1;
            }
        
            ChangeCar(previewedCar);
        }

        private void PreviewPreviousCar()
        {
            if (previewedCar == 0)
            {
                previewedCar = carList.Count - 1;
            }
            else
            {
                previewedCar -= 1;
            }

            ChangeCar(previewedCar);
        }

        private void ChangeCar(int selectedCarIndex)
        {
            if (carInstance != null)
                Destroy(carInstance);
            car = carList[selectedCarIndex];
            carInstance = Instantiate(car.carPrefab, spawn);
            nameText.SetText(car.carInfo.CarName);
            carShopPage.SetPreviewedCarInfo(car.carInfo);

            carInstance.GetComponent<CarTuning>().Data.ApplyTuning();
        }

        private void OnDisable()
        {
            rightButton.onClick.RemoveAllListeners();
            leftButton.onClick.RemoveAllListeners();
        }
    }
}
