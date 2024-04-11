using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Multiplayer : MonoBehaviourPunCallbacks
{
    [SerializeField] private Game game;
    [SerializeField] private TMP_Text waitingForPlayersText;
    private Spawner _spawner;
    private Hashtable _customProperties = new();
    private int _maxPlayers = 4;
    private GameObject _car;
    private string _roomName;
    
    public void Setup(Spawner spawner)
    {
        enabled = true;
        this._spawner = spawner;

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
        PhotonNetwork.JoinOrCreateRoom(_roomName, new RoomOptions { MaxPlayers = _maxPlayers }, null);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);

        _roomName += 1;
        PhotonNetwork.JoinOrCreateRoom(_roomName, new RoomOptions { MaxPlayers = _maxPlayers }, null);

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
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        WaitForPlayers();
    }

    private void WaitForPlayers()
    {
        var roomPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        if (roomPlayerCount == _maxPlayers)
        {
            waitingForPlayersText.gameObject.SetActive(false);
            game.StartGame();
            _spawner.StartCar();
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        else
        {
            waitingForPlayersText.SetText($"{roomPlayerCount}/{_maxPlayers}");
        }
    }
}
