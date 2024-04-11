using System.Collections;
using TMPro;
using UnityEngine;

namespace Score
{
    public class ScoreSingleplayer : Score
    {
        public override void Init(TMP_Text text, PlayerListItem playerListItem, Transform playerListTransform)
        {
            base.Init(text, playerListItem, playerListTransform);
            playerListTransform.gameObject.SetActive(false);
        }

        protected override IEnumerator CountScore()
        {
            while (true)
            {
                if (CarController.IsDrifting)
                {
                    Text.SetText($"Score: {++count}");
                }
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
