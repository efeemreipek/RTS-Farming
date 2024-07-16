using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Action OnTimerEnd;

    private float timer = 0f;
    private float timeCountdown;
    private bool isTimerRunning = false;


    private void Update()
    {
        if (isTimerRunning)
        {
            timer += Time.deltaTime;

            if (timer >= timeCountdown)
            {
                TimerEnd();
            }
        }
    }

    public void StartTimer(float time)
    {
        timeCountdown = time;
        isTimerRunning = true;
    }

    private void TimerEnd()
    {
        timer = 0f;
        isTimerRunning = false;
        OnTimerEnd?.Invoke();
    }

    public float GetRemainingTimeNormalized() => (timeCountdown - timer) / timeCountdown;
}
