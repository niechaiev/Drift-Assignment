using TMPro;
using UnityEngine;

namespace Score
{
    public class ScoreInstantiator : MonoBehaviour
    {
        [SerializeField] private GameObject scoreObject;
    
        [Header("SpawnData")]
        [SerializeField] protected TMP_Text text;
        [SerializeField] protected PlayerListItem playerListItem;
        [SerializeField] protected Transform playerListTransform;

        public Score InitializeScore(bool isOnline)
        {
            Score score = isOnline switch
            {
                true => scoreObject.AddComponent<ScoreMultiplayer>(),
                false => scoreObject.AddComponent<ScoreSingleplayer>()
            };
            score.Init(text, playerListItem, playerListTransform);
            return score;
        }
    }
}