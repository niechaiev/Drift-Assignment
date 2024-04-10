using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Multiplayer : MonoBehaviourPunCallbacks
{
    [SerializeField] private Spawner spawner;
    [SerializeField] private Game game;
    [SerializeField] private TMP_Text waitingForPlayersText;
    
    private Hashtable customProperties = new();
    private int maxPlayers = 4;
    
    private GameObject car;
    private string roomName;
    
    public void Setup(Spawner spawner)
    {
        this.spawner = spawner;

        customProperties["Tuning"] = JsonUtility.ToJson(Player.Instance.CarTunings[Player.Instance.SelectedCar]);
        PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);
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
        roomName = SceneManager.GetActiveScene().name;
        PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions { MaxPlayers = maxPlayers }, null);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);

        roomName += 1;
        PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions { MaxPlayers = maxPlayers }, null);

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
        
        spawner.Spawn();
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
        if (roomPlayerCount == maxPlayers)
        {
            waitingForPlayersText.gameObject.SetActive(false);
            game.StartGame();
            spawner.StartCar();
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        else
        {
            waitingForPlayersText.SetText($"{roomPlayerCount}/{maxPlayers}");
        }
    }
}
