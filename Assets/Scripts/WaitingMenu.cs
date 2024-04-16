using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaitingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject waitingForPlayersGameObject;
    [SerializeField] private TMP_Text waitingForPlayersText;
    [SerializeField] private GameObject maxPlayersGameObject;
    [SerializeField] private Slider maxPlayersSlider;
    
    public int MaxPlayers
    {
        get => (int)maxPlayersSlider.value;
    }
    

    public void SetText(int roomPlayerCount, int maxPlayers)
    {
        waitingForPlayersText.SetText($"{roomPlayerCount}/{maxPlayers}");
    }
    
    public void ShowMaxPlayersSlider(Action<int> onSliderValueChanged)
    {
        maxPlayersGameObject.SetActive(true);
        maxPlayersSlider.onValueChanged.RemoveAllListeners();
        maxPlayersSlider.onValueChanged.AddListener(value =>
        {
            onSliderValueChanged?.Invoke((int)value);
        });
    }
}
