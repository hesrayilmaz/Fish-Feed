using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private CoinManager coinManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void BuyCharacter()
    {
        GameObject selectedButton = EventSystem.current.currentSelectedGameObject.gameObject; 
        
        int fishValue;
        int totalCoin = coinManager.GetCoin();
        int.TryParse(selectedButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text, out fishValue);

        if (fishValue <= totalCoin)
        {
            string selectedFish = selectedButton.transform.parent.name;

            GameObject previousSelected = GameObject.Find("Canvas").transform.Find("ShopButtonsController").transform.Find(PlayerPrefs.GetString("selectedCharacter")).gameObject;
            
            previousSelected.transform.Find("Selected").gameObject.SetActive(false);
            previousSelected.transform.Find("SelectButton").gameObject.SetActive(true);

            Debug.Log("WHICH FISH IS BOUGHT " + selectedFish);
            
            coinManager.SetCoin(totalCoin - fishValue);
            
            PlayerPrefs.SetString("selectedCharacter", selectedFish);
            string purchasedCharacters = PlayerPrefs.GetString("purchasedCharacters");
            purchasedCharacters += "," + selectedFish;
            PlayerPrefs.SetString("purchasedCharacters", purchasedCharacters);

            selectedButton.transform.parent.transform.Find("Selected").gameObject.SetActive(true);
            selectedButton.SetActive(false);
        }
    }

    public void SelectCharacter()
    {
        GameObject selectedButton = EventSystem.current.currentSelectedGameObject.gameObject;
        
        string selectedFish = selectedButton.transform.parent.name;
        Debug.Log("WHICH FISH IS SELECTED "+ selectedFish);

        GameObject previousSelected = GameObject.Find("Canvas").transform.Find("ShopButtonsController").transform.Find(PlayerPrefs.GetString("selectedCharacter")).gameObject;
        Debug.Log("PREVIOUS SELECTED: " + previousSelected);
        previousSelected.transform.Find("Selected").gameObject.SetActive(false);
        previousSelected.transform.Find("SelectButton").gameObject.SetActive(true);

        PlayerPrefs.SetString("selectedCharacter", selectedFish);
        selectedButton.transform.parent.transform.Find("Selected").gameObject.SetActive(true);
        selectedButton.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
