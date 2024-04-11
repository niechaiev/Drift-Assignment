using System.Collections;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using TMPro;
using UnityEngine;

namespace Score
{
    public class ScoreMultiplayer : Score
    {
        protected override IEnumerator CountScore()
        {
            while (true)
            {
                UpdateLeaderBoard();
                if (CarController.IsDrifting)
                {
                    Text.SetText($"Score: {++count}");
                    PhotonNetwork.LocalPlayer.SetScore(count);
                }
                yield return new WaitForFixedUpdate();
            }
        }
    
        private void UpdateLeaderBoard()
        {
            foreach (var player in PhotonNetwork.CurrentRoom.Players)
            {
                var item = PlayerListItems.Find(p => p.ActorNumber == player.Value.ActorNumber);
                if (item != null)
                    item.SetScoreText(player.Value.GetScore());
            }
        }
    
    
        public override void Init(TMP_Text text, PlayerListItem playerListItem, Transform playerListTransform)
        {
            base.Init(text, playerListItem, playerListTransform);
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
            var item = PlayerListItems.Find(p => p.ActorNumber == otherPlayer.ActorNumber);
            Destroy(item.gameObject);
            PlayerListItems.Remove(item);
        }

        private void CreatePlayerListItem(Photon.Realtime.Player newPlayer)
        {
            var newPlayerListItem = Instantiate(PlayerListItem, PlayerListTransform);
            newPlayerListItem.Setup(newPlayer.NickName == string.Empty ? "no name" : newPlayer.NickName, newPlayer.ActorNumber);
            PlayerListItems.Add(newPlayerListItem);
        }
    }
}
