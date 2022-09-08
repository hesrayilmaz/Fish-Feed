using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public static int score;

    private void Start()
    {
        score = 0;
    }
    private void Update()
    {
        scoreText.text = "" + score;
    }

    public void IncreaseScore(int point)
    {
        score += point;
    }
    public void DecreaseScore(int point)
    {
        if (score - point < 0)
            score = 0;
        else score -= point;
    }
}
