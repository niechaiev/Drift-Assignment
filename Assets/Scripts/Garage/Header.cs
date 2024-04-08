using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Garage
{
    public class Header : MonoBehaviour
    { 
        [SerializeField] private Button backButton;
        [SerializeField] private Button IAPShopButton;
        [SerializeField] private TMP_Text textGold;
        [SerializeField] private TMP_Text textCash;
        [SerializeField] private MainMenu mainMenu;
    
        public Button BackButton => backButton;

        private void Awake()
        {
            backButton.onClick.AddListener(() => mainMenu.gameObject.SetActive(true));
        
            textGold.SetText($"{Player.Instance.Gold} \u2666");
            textCash.SetText($"{Player.Instance.Cash} $");
        
            Player.Instance.OnGoldChange = gold => textGold.SetText($"{gold} $");
            Player.Instance.OnCashChange = cash => textCash.SetText($"{cash} $");
        }

        public void ShowButtonBack(bool state)
        {
            backButton.gameObject.SetActive(state);
        }
    }
}
