using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        textGold.SetText($"{PlayerPrefs.GetInt("gold", 0)} \u2666");
        textCash.SetText($"{PlayerPrefs.GetInt("cash", 0)} $");
    }

    public void ShowButtonBack(bool state)
    {
        backButton.gameObject.SetActive(state);
    }
}
