using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoubleReward : MonoBehaviour
{
    [SerializeField] private TMP_Text cashText;
    [SerializeField] private Button doubleButton;
    [SerializeField] private Button closeButton;
    

    private void Awake()
    {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
    }

    public void Setup(int reward)
    {
        gameObject.SetActive(true);
        cashText.SetText($"{reward} $");
    }
}
