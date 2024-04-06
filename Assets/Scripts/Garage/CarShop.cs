using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarShop : Page
{
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button buyButton;
    private CarInfo previewedCarInfo;

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

    protected override void OnEnable()
    {
        base.OnEnable();
    }
}
