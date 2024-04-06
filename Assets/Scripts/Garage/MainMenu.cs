using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button carShopButton;
    [SerializeField] private Button tuningButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button selectLevelButton;
    [Header("Pages")] 
    [SerializeField] private LevelSelect levelSelect;
    [SerializeField] private CarShop carShop;
    [SerializeField] private CarSelect carSelect;
    
    
    private void Awake()
    {
        Application.targetFrameRate = 60;
        selectLevelButton.onClick.AddListener(OpenLevelSelect);
        carShopButton.onClick.AddListener(OpenCarShop);
    }

    private void OpenLevelSelect()
    {
        levelSelect.gameObject.SetActive(true);
        gameObject.SetActive(false);
        carSelect.gameObject.SetActive(false);
    }

    private void OpenCarShop()
    {
        carShop.gameObject.SetActive(true);
        gameObject.SetActive(false);
        carSelect.SetModePreviewCar();
        
    }

    private void OnEnable()
    {
        carSelect.gameObject.SetActive(true);
        carSelect.SetModeSelectCar();
    }
}
