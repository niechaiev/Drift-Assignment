using System;
using System.Linq;
using TMPro;
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
        private Car car;
        
        public Car Car => car;
        
        public void SetModeSelectCar()
        {
            rightButton.onClick.RemoveListener(PreviewNextCar);
            leftButton.onClick.RemoveListener(PreviewPreviousCar);
            rightButton.onClick.AddListener(SelectNextCar);
            leftButton.onClick.AddListener(SelectPreviousCar);
            ChangeCar(Player.SelectedCar);
            previewedCar = Player.SelectedCar;
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
            if (Player.SelectedCar == Player.OwnedCars.Count - 1)
            {
                Player.SelectedCar = Player.OwnedCars[0];
            }
            else
            {
                var index = Player.OwnedCars.First(i => i == Player.SelectedCar);
                Player.SelectedCar = Player.OwnedCars[++index];
            }
        
            ChangeCar(Player.SelectedCar);
        }
    
        private void SelectPreviousCar()
        {
            if (Player.SelectedCar == Player.OwnedCars[0])
            {
                Player.SelectedCar = Player.OwnedCars.Count - 1;
            }
            else
            {
                var index = Player.OwnedCars.First(i => i == Player.SelectedCar);
                Player.SelectedCar = Player.OwnedCars[--index];
            }

            ChangeCar(Player.SelectedCar);
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
        }

        private void OnDisable()
        {
            rightButton.onClick.RemoveAllListeners();
            leftButton.onClick.RemoveAllListeners();
        }
    }
}
