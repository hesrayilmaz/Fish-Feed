using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

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
        GameObject previousCharacter = GameObject.Find("Canvas").transform.Find("Buttons").transform.GetChild(PlayerPrefs.GetInt("selectedCharacter")).gameObject;
        int fishValue;
        int selectedFish;
        int totalCoin = coinManager.GetCoin();
        int.TryParse(selectedButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text, out fishValue);
        if (fishValue <= totalCoin)
        {
            string buttonParent = selectedButton.transform.parent.name;
            Debug.Log("WHICH FISH IS BOUGHT " + buttonParent);
            int.TryParse(buttonParent.Substring(4), out selectedFish);
            coinManager.SetCoin(totalCoin - fishValue);
            Debug.Log("FISH NUMBER " + selectedFish);
            PlayerPrefs.SetInt("selectedCharacter", selectedFish);
            selectedButton.SetActive(false);
            previousCharacter.transform.Find("Selected").gameObject.SetActive(false);
            previousCharacter.transform.Find("SelectButton").gameObject.SetActive(true);
        }
    }

    public void SelectCharacter()
    {
        GameObject selectedButton = EventSystem.current.currentSelectedGameObject.gameObject;
        int selectedFish;
        string buttonParent = selectedButton.transform.parent.name;
        Debug.Log("WHICH FISH IS SELECTED "+ buttonParent);
        int.TryParse(buttonParent.Substring(4), out selectedFish);
        Debug.Log("FISH NUMBER " + selectedFish);
        PlayerPrefs.SetInt("selectedCharacter", selectedFish);
    }
}
