using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Multiplayer : MonoBehaviourPunCallbacks
{
    [SerializeField] private Game game;
    [SerializeField] private TMP_Text waitingForPlayersText;
    [SerializeField] private GameObject maxPlayersGameObject;
    [SerializeField] private Slider maxPlayersSlider;
    private Spawner _spawner;
    private readonly Hashtable _customProperties = new();
    private GameObject _car;
    private string _roomName;
    private bool _gameStarted;
    
    public void Setup(Spawner spawner)
    {
        enabled = true;
        _spawner = spawner;

        _customProperties["Tuning"] = JsonUtility.ToJson(Player.Instance.CarTunings[Player.Instance.SelectedCar]);
        PhotonNetwork.LocalPlayer.SetCustomProperties(_customProperties);
        PhotonNetwork.LocalPlayer.NickName = Player.Instance.Nickname;
        
        Debug.Log("Connecting");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        
        Debug.Log("Connected to master");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        
        Debug.Log("in lobby");
        _roomName = SceneManager.GetActiveScene().name;
        PhotonNetwork.JoinOrCreateRoom(_roomName, new RoomOptions { MaxPlayers = (int)maxPlayersSlider.value }, null);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);

        _roomName += 1;
        PhotonNetwork.JoinOrCreateRoom(_roomName, new RoomOptions { MaxPlayers = (int)maxPlayersSlider.value }, null);

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("OnCreateRoomFailed");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        
        Debug.Log("in room");
        
        _spawner.Spawn();
        game.SetupGame();
        
        waitingForPlayersText.gameObject.SetActive(true);
        WaitForPlayers();

        if (PhotonNetwork.IsMasterClient)
            ShowMaxPlayersSlider();
    }

    private void ShowMaxPlayersSlider()
    {
        maxPlayersGameObject.SetActive(true);
        maxPlayersSlider.onValueChanged.RemoveAllListeners();
        maxPlayersSlider.onValueChanged.AddListener(value =>
        {
            PhotonNetwork.CurrentRoom.MaxPlayers = (int)value;
            WaitForPlayers();
        });
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        WaitForPlayers();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        WaitForPlayers();
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        WaitForPlayers();
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);

        if (PhotonNetwork.IsMasterClient)
            ShowMaxPlayersSlider();
    }

    private void WaitForPlayers()
    {
        if(_gameStarted) return;
        
        var roomPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        var maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;
        if (roomPlayerCount == maxPlayers)
        {
            _gameStarted = true;
            waitingForPlayersText.gameObject.SetActive(false);
            game.StartGame();
            _spawner.StartCar();
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        else
        {
            waitingForPlayersText.SetText($"{roomPlayerCount}/{maxPlayers}");
        }
    }
}
