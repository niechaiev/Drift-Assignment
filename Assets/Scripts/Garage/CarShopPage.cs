using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Garage
{
    public class CarShopPage : Page
    {
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private Button buyButton;
        private CarInfo previewedCarInfo;

        private void Awake()
        {
            buyButton.onClick.AddListener(BuyCar);
        }

        public void SetPreviewedCarInfo(CarInfo carInfo)
        {
            previewedCarInfo = carInfo;
            if (Player.Instance.OwnedCars.Any(c => c == carInfo.CarId))
            {
                priceText.SetText("Owned");
                buyButton.interactable = false;
                return;
            }
            
            priceText.SetText(carInfo.Price.ToString());
            if (previewedCarInfo.IsCurrencyGold)
            {
                priceText.color = Color.green;
                priceText.SetText(priceText.text + " \u2666");
                buyButton.interactable = Player.Instance.Gold >= carInfo.Price;
            }
            else
            {
                priceText.color = Color.white;
                priceText.SetText(priceText.text + " $");
                buyButton.interactable = Player.Instance.Cash >= carInfo.Price;
            }
        }

        private void BuyCar()
        {
            Player.Instance.OwnedCarsAdd(previewedCarInfo.CarId);
            Player.Instance.Cash -= previewedCarInfo.Price;
            priceText.SetText("Owned");
            buyButton.interactable = false;
        }
    }
}