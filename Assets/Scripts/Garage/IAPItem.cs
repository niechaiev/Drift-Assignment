using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Garage
{
    public class IAPItem : MonoBehaviour

    {
        [SerializeField] private Button iapButton;
        [SerializeField] private TMP_Text amountText;
        [SerializeField] private TMP_Text priceText;
        private bool _isGold;
        private int _amount;
        public Action<bool, int> OnClick;

        public void Setup(bool isGold, int amount, string price)
        {
            _isGold = isGold;
            _amount = amount;

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
            iapButton.onClick.AddListener(() =>
            {
                OnClick?.Invoke(_isGold, _amount);
            });
        }
    
    
    }
}
