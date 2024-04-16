using MyExtensions;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button menuButton;
    [SerializeField] private Button leaveButton;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private DoubleReward doubleReward;

    private void Awake()
    {
        menuButton.onClick.AddListener(() =>
        {
            Show(!canvasGroup.blocksRaycasts);
        });
        leaveButton.onClick.AddListener(() =>
        {
            PhotonNetwork.LocalPlayer.SetScore(0);
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("Garage");
        });
    }

    private void Start()
    {
        doubleReward.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Show(!canvasGroup.blocksRaycasts);
        }
    }

    public void Show(bool state = true)
    {
        canvasGroup.ShowCanvasGroup(state);
    }

    public void ShowReward(int reward)
    {
        doubleReward.Setup(reward);
    }
}
