using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoubleReward : MonoBehaviour
{
    [SerializeField] private TMP_Text cashText;
    [SerializeField] private Button doubleButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private RewardedVideo rewardedVideo;
    private int _reward;
    

    private void Awake()
    {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        doubleButton.onClick.AddListener(() => rewardedVideo.ShowRewardedVideo());
        rewardedVideo.OnAdRewarded = () =>
        {
            cashText.SetText($"{_reward} $ x 2");
            doubleButton.interactable = false;
            Player.Instance.Cash += _reward;
            GAManager.OnMoneyGain(false, _reward, "rewardedVideo", "empty");
        };
    }

    public void Setup(int reward)
    {
        _reward = reward;
        gameObject.SetActive(true);
        cashText.SetText($"{reward} $");
    }
}
