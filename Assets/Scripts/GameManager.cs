using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    private GameObject gameOverPanel;
    public bool isPlaying = true;

    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel = GameObject.Find("Canvas").transform.Find("GameOverPanel").gameObject;
    }

    public void ShowPanel()
    {
        isPlaying = false;
        gameOverPanel.SetActive(true);
    }
    public void HidePanel()
    {
        gameOverPanel.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //isPlaying = true;
    }

    public void ResumeGame()
    {
        StartCoroutine(Resume());
    }

    IEnumerator Resume()
    {
        HidePanel();
        yield return new WaitForSeconds(3f);
        isPlaying = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
