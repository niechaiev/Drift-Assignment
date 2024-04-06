using System.Collections;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private int count;
    private CarController carController;
    public int Count => count;

    public void Setup(CarController carController)
    {
        this.carController = carController;
        StartCoroutine(CountScore());
    }

    private IEnumerator CountScore()
    {
        while (true)
        {
            if (carController.IsDrifting)
            {
                text.SetText($"Score: {++count}");
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
