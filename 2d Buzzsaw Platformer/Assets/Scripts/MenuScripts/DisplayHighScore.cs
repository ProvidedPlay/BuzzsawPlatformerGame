using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DisplayHighScore : MonoBehaviour
{
    public bool hasScoreText;
    public bool scoreUnlocked;
    public bool isRunHighScore;
    public int levelIndex;

    GameController gameController;
    public TextMeshProUGUI[] textBoxes;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        textBoxes = GetComponentsInChildren<TextMeshProUGUI>();
        if (textBoxes != null && textBoxes.Length>0)
        {
            UnpackTextBoxes();
        }
    }
    void UnpackTextBoxes()
    {
        nameText = textBoxes[0];
        if (hasScoreText && textBoxes.Length>1)
        {
            scoreText = textBoxes[1];
        }
    }
    private void OnEnable()
    {
        CheckIfHighScoreUnlocked();
    }
    void CheckIfHighScoreUnlocked()
    {
        if (!isRunHighScore)
        {
            scoreUnlocked = gameController.bestLevelTimes.ContainsKey(levelIndex);
            nameText.enabled = scoreUnlocked;
            if (scoreText != null)
            {
                scoreText.enabled = scoreUnlocked;
                if (scoreUnlocked)
                {
                    DisplayCurrentLevelHighScore();
                }
            }
        }
        if (isRunHighScore)
        {
            scoreUnlocked = gameController.bestRunTime == 0 ? false : true;

            nameText.enabled = scoreUnlocked;
            scoreText.enabled = scoreUnlocked;
            DisplayCurrentRunHighScore();
        }
        
    }
    void DisplayCurrentLevelHighScore()
    {
        scoreText.text = gameController.bestLevelTimes[levelIndex].ToString();
    }
    void DisplayCurrentRunHighScore()
    {
        
        scoreText.text = gameController.bestRunTime.ToString();
        if (gameController.bestRunTime == 0)
        {
            scoreText.text = "";
        }
    }
}
