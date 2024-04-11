using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    private float _timeLeft;
    public Action OnTimeOut;

    public void StartTimer(float time)
    {
        _timeLeft = time;
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        while (true)
        {
            _timeLeft -= Time.deltaTime;
            if (_timeLeft < 0)
            {
                OnTimeOut?.Invoke();
                yield break;
            }

            timerText.SetText($"{Mathf.FloorToInt(_timeLeft / 60)}:{Mathf.FloorToInt(_timeLeft % 60):00}");
            yield return null;
        }
    }
}
