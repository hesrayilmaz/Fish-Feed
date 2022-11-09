using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    private int coin;

    private void Start()
    {
        coin = PlayerPrefs.GetInt("coin");
        //coin = 0;
    }
    private void Update()
    {
        coinText.text = "" + PlayerPrefs.GetInt("coin");
    }

    public void IncreaseCoin(int point)
    {
        coin = PlayerPrefs.GetInt("coin");
        coin += point;
        PlayerPrefs.SetInt("coin", coin);
    }
    public void DecreaseCoin(int point)
    {
        coin = PlayerPrefs.GetInt("coin");
        if (coin - point < 0)
            coin = 0;
        else coin -= point;
        PlayerPrefs.SetInt("coin", coin);
    }

    public void SetCoin(int coin)
    {
        PlayerPrefs.SetInt("coin", coin);
    }

    public int GetCoin()
    {
        return PlayerPrefs.GetInt("coin");
    }
}
