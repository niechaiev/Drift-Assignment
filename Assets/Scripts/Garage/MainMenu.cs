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
        [Header("Pages")] 
        [SerializeField] private LevelSelectPage levelSelectPage;
        [SerializeField] private CarShopPage carShopPage;
        [SerializeField] private TuningPage tuningPage;
        [SerializeField] private SettingsPage settingsPage;
        [SerializeField] private CarSelect carSelect;
    
    
        private void Awake()
        {
            Application.targetFrameRate = 165;
            selectLevelButton.onClick.AddListener(OpenLevelSelect);
            carShopButton.onClick.AddListener(OpenCarShop);
            settingsButton.onClick.AddListener(OpenSettings);
            tuningButton.onClick.AddListener(OpenTuningPage);
        }
        
        private void OpenCarShop()
        {
            carShopPage.gameObject.SetActive(true);
            gameObject.SetActive(false);
            carSelect.SetModePreviewCar();
        }
        
        private void OpenLevelSelect()
        {
            DisableMainMenuAndSelectCar();
            levelSelectPage.gameObject.SetActive(true);
        }
        private void OpenTuningPage()
        {
            DisableMainMenuAndSelectCar();
            tuningPage.Setup(carSelect.Car.carInfo.CarTuning);
        }
        
        private void OpenSettings()
        {
            DisableMainMenuAndSelectCar();
            settingsPage.gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            carSelect.gameObject.SetActive(true);
            carSelect.SetModeSelectCar();
        }

        private void DisableMainMenuAndSelectCar()
        {
            gameObject.SetActive(false);
            carSelect.gameObject.SetActive(false);
        }
    }
}
