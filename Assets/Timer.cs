using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    private float timeLeft;
    public Action OnTimeOut;

    public void StartTimer(float time)
    {
        timeLeft = time;
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        while (true)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                OnTimeOut?.Invoke();
                yield break;
            }

            timerText.SetText($"{Mathf.FloorToInt(timeLeft / 60)}:{Mathf.FloorToInt(timeLeft % 60):00}");
            yield return null;
        }
    }
}
