using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoubleReward : MonoBehaviour
{
    [SerializeField] private TMP_Text cashText;
    [SerializeField] private Button doubleButton;

    private void Awake()
    {
    }

    public void Setup(int reward)
    {
        cashText.SetText($"{reward} $");
    }
}
