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
            if (Player.OwnedCars.Any(c => c == carInfo.Index))
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
                buyButton.interactable = Player.Gold >= carInfo.Price;
            }
            else
            {
                priceText.color = Color.white;
                priceText.SetText(priceText.text + " $");
                buyButton.interactable = Player.Cash >= carInfo.Price;
            }
        }

        private void BuyCar()
        {
            Player.OwnedCarsAdd(previewedCarInfo.Index);
            Player.Cash -= previewedCarInfo.Price;
            priceText.SetText("Owned");
            buyButton.interactable = false;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }
    }
}
