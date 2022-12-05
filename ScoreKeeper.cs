using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    private int correctAnswers;
    private int questionsSeen;

    public void IncrementCorrectAnswers()
    {
        correctAnswers++;
    }
    
    public void IncrementQuestionsSeen()
    {
        questionsSeen++;
    }

    public int CalculateScore()
    {
        return Mathf.RoundToInt(100 * (float)correctAnswers / questionsSeen);
    }
}
