using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButtonsController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        for (int i=0; i<transform.childCount; i++)
        {
            transform.GetChild(i).transform.Find("BuyButton").gameObject.SetActive(true);
        }

        //Debug.Log("PlayerPrefs.GetString(purchasedCharacters) "+PlayerPrefs.GetString("purchasedCharacters"));

        string[] purchasedCharacters = PlayerPrefs.GetString("purchasedCharacters", "Fish1").Split(",");
        
        foreach (string fish in purchasedCharacters)
        {
            //Debug.Log("fish name: " + fish);
            transform.Find(fish).transform.Find("BuyButton").gameObject.SetActive(false);
            transform.Find(fish).transform.Find("SelectButton").gameObject.SetActive(true);
        }

        Transform selectedCharacter = transform.Find(PlayerPrefs.GetString("selectedCharacter", "Fish1")).transform;
        selectedCharacter.Find("Selected").gameObject.SetActive(true);
        selectedCharacter.Find("SelectButton").gameObject.SetActive(false);
    }


}
