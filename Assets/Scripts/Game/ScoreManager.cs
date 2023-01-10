using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    [SerializeField] private TextMeshProUGUI gameOverHighScoreText;
    private int score;
    private int highScore;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "" + score;
    }

    public void IncreaseScore()
    {
        score += 1;
        if (score >= highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }

    public void SetGameOverScore()
    {
        gameOverScoreText.text = "Score: " + score;
        gameOverHighScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore");
    }
        
}
