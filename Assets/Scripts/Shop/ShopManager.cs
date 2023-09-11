using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private CoinManager coinManager;
    [SerializeField] private AudioSource clickAudio;
    [SerializeField] private AudioSource wrongClickAudio;

    public void BuyCharacter()
    {
        GameObject selectedButton = EventSystem.current.currentSelectedGameObject.gameObject;

        int fishValue;
        int totalCoin = coinManager.GetCoin();
        int.TryParse(selectedButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text, out fishValue);

        if (fishValue <= totalCoin)
        {
            clickAudio.Play();
            int selectedFish = selectedButton.transform.parent.GetSiblingIndex();

            GameObject previousSelected = GameObject.Find("Canvas").transform.Find("Fishes").transform.GetChild(PlayerPrefs.GetInt("selectedCharacter", 0)).gameObject;

            //Debug.Log("previousSelected: "+ previousSelected.name);
            previousSelected.transform.Find("Selected").gameObject.SetActive(false);
            previousSelected.transform.Find("SelectButton").gameObject.SetActive(true);

            coinManager.SetCoin(totalCoin - fishValue);

            PlayerPrefs.SetInt("selectedCharacter", selectedFish);
            PlayerPrefs.SetString(selectedFish + "Purchased", "true");

            selectedButton.transform.parent.transform.Find("Selected").gameObject.SetActive(true);
            selectedButton.SetActive(false);
        }
        else
        {
            wrongClickAudio.Play();
        }

    }

    public void SelectCharacter()
    {
        clickAudio.Play();

        GameObject selectedButton = EventSystem.current.currentSelectedGameObject.gameObject;
        int selectedFish = selectedButton.transform.parent.GetSiblingIndex();

        GameObject previousSelected = GameObject.Find("Canvas").transform.Find("Fishes").transform.GetChild(PlayerPrefs.GetInt("selectedCharacter", 0)).gameObject;

        previousSelected.transform.Find("Selected").gameObject.SetActive(false);
        previousSelected.transform.Find("SelectButton").gameObject.SetActive(true);

        PlayerPrefs.SetInt("selectedCharacter", selectedFish);
        selectedButton.transform.parent.transform.Find("Selected").gameObject.SetActive(true);
        selectedButton.SetActive(false);
    }

    public void PlayGame()
    {
        clickAudio.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
