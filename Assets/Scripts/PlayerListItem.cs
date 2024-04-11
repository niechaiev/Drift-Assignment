using TMPro;
using UnityEngine;

public class PlayerListItem : MonoBehaviour
{
    [SerializeField] private TMP_Text nicknameText;
    [SerializeField] private TMP_Text scoreText;
    private int _actorNumber;

    public int ActorNumber => _actorNumber;

    public void Setup(string nickname, int actorNumber)
    {
        nicknameText.SetText(nickname);
        this._actorNumber = actorNumber;
    }

    public void SetScoreText(int score)
    {
        scoreText.SetText(score.ToString());
    }


}
