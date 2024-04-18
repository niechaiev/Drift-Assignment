using Garage.FirebaseMenu;
using UnityEngine;
using UnityEngine.UI;

namespace Garage
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button carShopButton;
        [SerializeField] private Button tuningButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button selectLevelButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Button firebaseButton;
        
        
        [Header("Pages")] 
        [SerializeField] private LevelSelectPage levelSelectPage;
        [SerializeField] private CarShopPage carShopPage;
        [SerializeField] private TuningPage tuningPage;
        [SerializeField] private SettingsPage settingsPage;
        [SerializeField] private CarSelector carSelector;
        [SerializeField] private FirebaseContextPage firebasePage;
        
    
    
        private void Awake()
        {
            selectLevelButton.onClick.AddListener(OpenLevelSelect);
            carShopButton.onClick.AddListener(OpenCarShop);
            settingsButton.onClick.AddListener(OpenSettings);
            tuningButton.onClick.AddListener(OpenTuningPage);
            quitButton.onClick.AddListener(Application.Quit);
            firebaseButton.onClick.AddListener(OpenFirebasePage);
            Application.targetFrameRate = 60;
        }
        
        private void OpenFirebasePage()
        {
            DisableMainMenuAndSelectCar();
            firebasePage.gameObject.SetActive(true);
        }
        
        private void OpenCarShop()
        {
            carShopPage.gameObject.SetActive(true);
            gameObject.SetActive(false);
            carSelector.SetModePreviewCar();
        }
        
        private void OpenLevelSelect()
        {
            DisableMainMenuAndSelectCar();
            levelSelectPage.gameObject.SetActive(true);
        }
        private void OpenTuningPage()
        {
            DisableMainMenuAndSelectCar();
            tuningPage.Setup(carSelector.CarInstance);
        }
        
        private void OpenSettings()
        {
            DisableMainMenuAndSelectCar();
            settingsPage.gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            carSelector.gameObject.SetActive(true);
            carSelector.SetModeSelectCar();
        }

        public void DisableAllPages()
        {
            DisableMainMenuAndSelectCar();
            carShopPage.gameObject.SetActive(false);
            settingsPage.gameObject.SetActive(false);
            tuningPage.gameObject.SetActive(false);
            levelSelectPage.gameObject.SetActive(false);
        }

        public void DisableMainMenuAndSelectCar()
        {
            gameObject.SetActive(false);
            carSelector.gameObject.SetActive(false);
        }
    }
}
