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

        Player.Gold = PlayerPrefs.GetInt("gold", 0);
        Player.Cash = PlayerPrefs.GetInt("cash", 0);
        var ownedCars = PlayerPrefs.GetString("ownedCars").Split(new []{"#"}, StringSplitOptions.None);
        if (ownedCars.Length == 1) ownedCars[0] = "0";
        Player.OwnedCars = Array.ConvertAll(ownedCars, int.Parse);
        Player.SelectedCar = PlayerPrefs.GetInt("selectedCar", 0);
        
        textGold.SetText($"{Player.Gold} \u2666");
        textCash.SetText($"{Player.Cash} $");
    }

    public void ShowButtonBack(bool state)
    {
        backButton.gameObject.SetActive(state);
    }
}
