using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private GameObject gameOverPanel;
    public bool isPlaying = true;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel = GameObject.Find("Canvas").transform.Find("GameOverPanel").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPanel()
    {
        isPlaying = false;
        gameOverPanel.SetActive(true);
    }
    public void HidePanel()
    {
        isPlaying = true;
        gameOverPanel.SetActive(false);
    }
}
