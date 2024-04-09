using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Garage
{
    public class IAPItem : MonoBehaviour

    {
        [SerializeField] private Button IAPButton;
        [SerializeField] private TMP_Text amountText;
        [SerializeField] private TMP_Text priceText;
        private bool isGold;
        private int amount;
        public Action<bool, int> OnClick;

        public void Setup(bool isGold, int amount, string price)
        {
            this.isGold = isGold;
            this.amount = amount;

            var amountString = amount.ToString();
            if (isGold)
            {
                amountString += " \u2666";
                amountText.color = Color.green;
            }
            else
            {
                amountString += " $";
                amountText.color = Color.white;
            }

            amountText.SetText(amountString);
            priceText.SetText(price);
            IAPButton.onClick.AddListener(() =>
            {
                OnClick?.Invoke(this.isGold, this.amount);
            });
            gameObject.SetActive(true);
        }
    
    
    }
}
