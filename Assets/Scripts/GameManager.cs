using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject countdown;
    [SerializeField] private GameObject player;

    public bool isPlaying = true;

    // Start is called before the first frame update
    void Start()
    {

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
    }

    public void ResumeGame()
    {
        StartCoroutine(Resume());
    }

    IEnumerator Resume()
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
        player.GetComponent<CapsuleCollider>().enabled = false;
        isPlaying = true;
        yield return new WaitForSeconds(2f);
        player.GetComponent<CapsuleCollider>().enabled = true;
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
