using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    public static int score;

    private void Start()
    {
        score = PlayerPrefs.GetInt("score");
    }
    private void Update()
    {
        scoreText.text = "" + score;
    }

    public void IncreaseScore(int point)
    {
        score += point;
        PlayerPrefs.SetInt("score", score);
    }
    public void DecreaseScore(int point)
    {
        if (score - point < 0)
            score = 0;
        else score -= point;
        PlayerPrefs.SetInt("score", score);
    }
}
