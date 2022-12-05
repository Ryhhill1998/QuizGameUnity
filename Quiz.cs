using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] private List<QuestionSO> questions = new List<QuestionSO>();
    [SerializeField] private TextMeshProUGUI questionText;
    private QuestionSO currentQuestion;

    [Header("Answers")]
    [SerializeField] private int correctAnswerIndex;
    [SerializeField] private GameObject[] answerButtons;
    
    [Header("Button Colours")]
    [SerializeField] private Sprite defaultAnswerSprite;
    [SerializeField] private Sprite correctAnswerSprite;
    
    [Header("Timer")]
    [SerializeField] private Image timerImage;
    private Timer timer;
    private bool questionAnsweredEarly;

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI scoreText;
    private ScoreKeeper scoreKeeper;

    [Header("Progress Bar")]
    [SerializeField] private Slider progressBar;

    public bool isComplete;
    
    private EndQuiz endQuiz;

    private void Awake()
    {
        timer = FindObjectOfType<Timer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        endQuiz = FindObjectOfType<EndQuiz>();
    }

    private void Start()
    {
        InitialiseProgressBar();
    }

    void Update()
    {
        UpdateTimerImage();
        
        if (timer.loadNextQuestion)
        {
            questionAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if (timer.timerRanOut && !questionAnsweredEarly)
        {
            OnTimerRunOut();
        }
    }

    private void GetNextQuestion()
    {
        if (questions.Count == 0)
        {
            isComplete = true;
            endQuiz.UpdateFinalScoreText();
            return;
        }
        
        GetRandomQuestion();
        SetButtonState(true);
        InitialiseQuestion();
        ResetButtonSprite();

        UpdateProgressBar();
    }

    private void GetRandomQuestion()
    {
        int randomIndex = Random.Range(0, questions.Count);
        currentQuestion = questions[randomIndex];
        questions.RemoveAt(randomIndex);
    }

    private void InitialiseQuestion()
    {
        questionText.text = currentQuestion.GetQuestion();
        correctAnswerIndex = currentQuestion.GetCorrectAnswerIndex();
        
        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestion.GetAnswer(i);
        }
    }

    private void InitialiseProgressBar()
    {
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    private void UpdateProgressBar()
    {
        progressBar.value++;
    }

    public void OnAnswerSelected(int buttonIndex)
    {
        DisplayAnswer(buttonIndex);
        SetButtonState(false);
        timer.CancelTimer();
        questionAnsweredEarly = true;
    }

    private void OnTimerRunOut()
    {
        DisplayAnswer(-1);
        SetButtonState(false);
    }

    private void DisplayAnswer(int buttonIndex)
    {
        if (buttonIndex == correctAnswerIndex)
        {
            questionText.text = "Correct!";
            scoreKeeper.IncrementCorrectAnswers();
        }
        else
        {
            questionText.text = "Incorrect! The answer was " + currentQuestion.GetAnswer(correctAnswerIndex);
        }

        UpdateCorrectButtonSprite();
        scoreKeeper.IncrementQuestionsSeen();
        UpdateScoreText();
    }

    private void UpdateCorrectButtonSprite()
    {
        Image buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
        buttonImage.sprite = correctAnswerSprite;
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + scoreKeeper.CalculateScore() + "%";
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
