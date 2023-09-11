using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ShopButtonsController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //ResetToDefault();

        //Set first character as purchased as default 
        PlayerPrefs.SetString(0 + "Purchased", "true");

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).transform.Find("Selected").gameObject.SetActive(false);

            if (PlayerPrefs.GetString(i + "Purchased", "false") == "true")
            {
                transform.GetChild(i).transform.Find("BuyButton").gameObject.SetActive(false);
                transform.GetChild(i).transform.Find("SelectButton").gameObject.SetActive(true);
            }
            else if (PlayerPrefs.GetString(i + "Purchased", "false") == "false")
            {
                transform.GetChild(i).transform.Find("BuyButton").gameObject.SetActive(true);
                transform.GetChild(i).transform.Find("SelectButton").gameObject.SetActive(false);
            }
        }

        Transform selectedCharacter = transform.GetChild(PlayerPrefs.GetInt("selectedCharacter", 0)).transform;
        selectedCharacter.Find("Selected").gameObject.SetActive(true);
        selectedCharacter.Find("SelectButton").gameObject.SetActive(false);
    }


    private void ResetToDefault()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).transform.Find("Selected").gameObject.SetActive(false);
            transform.GetChild(i).transform.Find("BuyButton").gameObject.SetActive(true);
            transform.GetChild(i).transform.Find("SelectButton").gameObject.SetActive(false);
            PlayerPrefs.SetString(i + "Purchased", "false");
        }
    }

}
