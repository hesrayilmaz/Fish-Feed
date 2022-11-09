using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButtonsController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        PlayerPrefs.SetString("selectedCharacter", "Fish1");
        PlayerPrefs.SetString("purchasedCharacters", "Fish1");

        for (int i=0; i<transform.childCount; i++)
        {
            transform.GetChild(i).transform.Find("BuyButton").gameObject.SetActive(true);
        }

        Debug.Log("PlayerPrefs.GetString(purchasedCharacters) "+PlayerPrefs.GetString("purchasedCharacters"));
        if (PlayerPrefs.GetString("purchasedCharacters") != "")
        {
            string[] purchasedCharacters = PlayerPrefs.GetString("purchasedCharacters").Split(",");
            foreach (string fish in purchasedCharacters)
            {
                Debug.Log("fish name: " + fish);
                transform.Find(fish).transform.Find("BuyButton").gameObject.SetActive(false);
                transform.Find(fish).transform.Find("SelectButton").gameObject.SetActive(true);
            }
        }
        
        transform.Find(PlayerPrefs.GetString("selectedCharacter")).transform.Find("Selected").gameObject.SetActive(true);
        transform.Find(PlayerPrefs.GetString("selectedCharacter")).transform.Find("SelectButton").gameObject.SetActive(false);
    }


}
