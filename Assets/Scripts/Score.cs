using System.Collections;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private int count;
    private CarController carController;
    private Coroutine coroutine;
    public int Count => count;

    public void Setup(CarController carController)
    {
        this.carController = carController;
        coroutine = StartCoroutine(CountScore());
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

    private void OnDisable()
    {
        StopCoroutine(coroutine);
    }
}
