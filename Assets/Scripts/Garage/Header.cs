using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Garage
{
    public class Header : MonoBehaviour
    { 
        [SerializeField] private Button backButton;
        [SerializeField] private Button iapButton;
        [SerializeField] private TMP_Text textGold;
        [SerializeField] private TMP_Text textCash;
        [SerializeField] private MainMenu mainMenu;
        [SerializeField] private IAPPage iapPage;
    
        public Button BackButton => backButton;

        private void Awake()
        {
            backButton.onClick.AddListener(() => mainMenu.gameObject.SetActive(true));
        
            textGold.SetText($"{Player.Instance.Gold} \u2666");
            textCash.SetText($"{Player.Instance.Cash} $");
        
            Player.Instance.OnGoldChange = gold => textGold.SetText($"{gold} \u2666");
            Player.Instance.OnCashChange = cash => textCash.SetText($"{cash} $");
            
            iapButton.onClick.AddListener(()=>
            {
                iapPage.Setup();
                mainMenu.DisableAllPages();
                ShowButtonBack(true);
            });
        }

        public void ShowButtonBack(bool state)
        {
            backButton.gameObject.SetActive(state);
        }
    }
}
