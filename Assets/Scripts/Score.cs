using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private int score;
    private CarController carController;
    
    private void Awake()
    {
        carController = GameObject.FindGameObjectWithTag("Player").GetComponent<CarController>();
    }

    private void FixedUpdate()
    {
        if (carController.IsDrifting)
        {
            text.SetText($"Score: {++score}");
        }
            
    }
}
