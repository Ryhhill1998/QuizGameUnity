using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndQuiz : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI finalScoreText;
    private ScoreKeeper scoreKeeper;

    void Awake()
    {
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    public void UpdateFinalScoreText()
    {
        finalScoreText.text = "Congratulations! Your final score was " + scoreKeeper.CalculateScore() + "%";
    }
}
