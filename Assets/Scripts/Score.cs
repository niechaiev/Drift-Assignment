using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private PlayerListItem playerListItem;
    [SerializeField] private Transform playerListTransform;
    private List<PlayerListItem> playerListItems = new();
    private int count;
    private CarController carController;
    private Coroutine coroutine;
    public int Count => count;

    public void Setup(CarController carController)
    {
        this.carController = carController;
        coroutine = StartCoroutine(CountScore());
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            CreatePlayerListItem(player.Value);
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        CreatePlayerListItem(newPlayer);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        var item = playerListItems.Find(p => p.ActorNumber == otherPlayer.ActorNumber);
        Destroy(item.gameObject);
        playerListItems.Remove(item);
    }

    private void CreatePlayerListItem(Photon.Realtime.Player newPlayer)
    {
        var newPlayerListItem = Instantiate(playerListItem, playerListTransform);
        newPlayerListItem.Setup(newPlayer.NickName == string.Empty ? "no name" : newPlayer.NickName, newPlayer.ActorNumber);
        playerListItems.Add(newPlayerListItem);
    }

    private IEnumerator CountScore()
    {
        while (true)
        {
            if(PhotonNetwork.IsConnected)
                UpdateLeaderBoard();
            if (carController.IsDrifting)
            {
                text.SetText($"Score: {++count}");
                PhotonNetwork.LocalPlayer.SetScore(count);
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private void UpdateLeaderBoard()
    {
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            var item = playerListItems.Find(p => p.ActorNumber == player.Value.ActorNumber);
            if (item != null)
                item.SetScoreText(player.Value.GetScore());
        }
    }

    public override void OnDisable()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        foreach (var item in playerListItems)
        {
            Destroy(item.gameObject);
        }
        playerListItems.Clear();
    }
}
