using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] private TextMeshProUGUI questionText;
    private QuestionSO currentQuestion;
    [SerializeField] private List<QuestionSO> questions = new List<QuestionSO>();

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
        if (questions.Count == 0) return;
        GetRandomQuestion();
        SetButtonState(true);
        InitialiseQuestion();
        ResetButtonSprite();
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
            questionText.text = "Incorrect! The answer was " + currentQuestion.GetAnswer(correctAnswerIndex);

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
