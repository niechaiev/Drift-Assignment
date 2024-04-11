using MyExtensions;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button leaveButton;
    [SerializeField] private CanvasGroup canvasGroup;
    

    private void Awake()
    {
        leaveButton.onClick.AddListener(() =>
        {
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("Garage");
        });
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
}
