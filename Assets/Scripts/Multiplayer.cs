using System;
using System.Collections;
using Photon.Pun;
using Tuning;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Multiplayer : MonoBehaviourPunCallbacks
{
    [SerializeField] private Spawner spawner;
    [SerializeField] private Game game;
    private Hashtable customProperties = new();
    
    private GameObject car;
    
    private void Start()
    {
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

        PhotonNetwork.JoinOrCreateRoom("test", null, null);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        
        Debug.Log("in room");
        
        customProperties["Tuning"] = JsonUtility.ToJson(Player.Instance.CarTunings[Player.Instance.SelectedCar]);
        PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);
        
        spawner.Spawn();
        game.SetupGame();
    }
}
