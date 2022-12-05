 using System;
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.SceneManagement;

 public class GameManager : MonoBehaviour
{
    private Quiz quiz;
    private EndQuiz endQuiz;

    private void Awake()
    {
        quiz = FindObjectOfType<Quiz>();
        endQuiz = FindObjectOfType<EndQuiz>();
    }

    void Start()
    {
        SetGameObjectStates(true);
    }
    
    void Update()
    {
        if (quiz.isComplete)
        {
            SetGameObjectStates(false);
        }
    }

    private void SetGameObjectStates(bool gameOn)
    {
        if (gameOn)
        {
            quiz.gameObject.SetActive(true);
            endQuiz.gameObject.SetActive(false);
        }
        else
        {
            quiz.gameObject.SetActive(false);
            endQuiz.gameObject.SetActive(true);
        }
    }

    public void OnReplayQuiz()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
