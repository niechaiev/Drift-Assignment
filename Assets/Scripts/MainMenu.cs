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
    
    private void Awake()
    {
        selectLevelButton.onClick.AddListener(OpenLevelSelect);
    }

    private void OpenLevelSelect()
    {
        levelSelect.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
