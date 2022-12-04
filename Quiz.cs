using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private QuestionSO question;

    [Header("Answers")]
    [SerializeField] private int correctAnswerIndex;
    [SerializeField] private GameObject[] answerButtons;
    
    [Header("Button Colours")]
    [SerializeField] private Sprite defaultAnswerSprite;
    [SerializeField] private Sprite correctAnswerSprite;
    
    [Header("Timer")]
    [SerializeField] private Image timerImage;
    private Timer timer;
    public bool questionAnsweredEarly;

    private void Start()
    {
        timer = FindObjectOfType<Timer>();
        GetNextQuestion();
    }

    void Update()
    {
        UpdateTimerImage();
        
        if (timer.loadNextQuestion)
        {
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if (timer.timerRanOut && !questionAnsweredEarly)
        {
            OnTimerRunout();
        }
    }

    private void GetNextQuestion()
    {
        SetButtonState(true);
        InitialiseQuestion();
        ResetButtonSprite();
    }

    private void InitialiseQuestion()
    {
        questionText.text = question.GetQuestion();
        correctAnswerIndex = question.GetCorrectAnswerIndex();
        
        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = question.GetAnswer(i);
        }
    }

    public void OnAnswerSelected(int buttonIndex)
    {
        DisplayAnswer(buttonIndex);
        SetButtonState(false);
        timer.CancelTimer();
        questionAnsweredEarly = true;
    }

    private void OnTimerRunout()
    {
        DisplayAnswer(-1);
        SetButtonState(false);
    }

    private void DisplayAnswer(int buttonIndex)
    {
        if (buttonIndex == correctAnswerIndex)
            questionText.text = "Correct!";
        else
            questionText.text = "Incorrect! The answer was " + question.GetAnswer(correctAnswerIndex);

        Image buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
        buttonImage.sprite = correctAnswerSprite;
    }

    private void SetButtonState(bool state)
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }

    private void ResetButtonSprite()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Image buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }
    
    private void UpdateTimerImage()
    {
        timerImage.fillAmount = timer.timerFillFraction;
    }
}
