using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject countdown;
    [SerializeField] private AudioSource backgroundAudio;

    private GameObject player;
    public bool isPlaying;
    private bool isStarted;

    // Start is called before the first frame update
    void Start()
    {
        isPlaying = false;
        isStarted = true;
        PlayerPrefs.SetString("EarnedHeart","false");
    }

    private void Update()
    {
        if (isStarted)
        {
            player = GameObject.Find("PlayerController").transform.GetComponentsInChildren<Transform>()[1].gameObject;
            Debug.Log("WHICH PLAYER IS ACTIVE: " + player.name);
            isStarted = false;
            StartCoroutine(Play());
        }
    }

    public void ShowPanel()
    {
        isPlaying = false;
        backgroundAudio.Stop();
        gameOverPanel.SetActive(true);
        if (PlayerPrefs.GetString("EarnedHeart") == "true")
            gameOverPanel.transform.Find("ExtraHeartButton").gameObject.SetActive(false);
    }
    public void HidePanel()
    {
        gameOverPanel.SetActive(false);
        backgroundAudio.Play();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResumeGame()
    {
        StartCoroutine(Play());
    }

    IEnumerator Play()
    {
        TextMeshProUGUI countdownText = countdown.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        HidePanel();
        countdown.SetActive(true);
        int counter = 3;

        while (counter > 0)
        {
            countdownText.text = "" + counter;
            yield return new WaitForSeconds(1);
            counter--;
        }
        countdown.SetActive(false);
        player.GetComponent<BoxCollider>().enabled = false;
        isPlaying = true;
        yield return new WaitForSeconds(2f);
        player.GetComponent<BoxCollider>().enabled = true;
    }

    public void GoToShop()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
