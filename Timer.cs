using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private float answerTime = 30f;
    [SerializeField] private float reviewTime = 10f;
    [SerializeField] private float timeRemaining;
    private float startingTime;
    
    public float timerFillFraction;

    public bool loadNextQuestion;
    private bool isAnsweringQuestion;
    public bool timerRanOut;

    void Update()
    {
        UpdateTimer();
    }

    public void CancelTimer()
    {
        timeRemaining = 0;
    }

    private void UpdateTimer()
    {
        timeRemaining -= Time.deltaTime;

        if (timeRemaining > 0)
        {
            UpdateTimerFillAmount();
            timerRanOut = false;
        }
        else if (isAnsweringQuestion)
        {
            isAnsweringQuestion = false;
            timeRemaining = startingTime = reviewTime;
            timerRanOut = true;
        }
        else
        {
            isAnsweringQuestion = true;
            timeRemaining = startingTime = answerTime;
            loadNextQuestion = true;
        }
    }

    private void UpdateTimerFillAmount()
    {
        timerFillFraction = timeRemaining / startingTime;
    }
}
