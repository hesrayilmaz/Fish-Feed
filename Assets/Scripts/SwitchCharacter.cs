using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCharacter : MonoBehaviour
{
    private int selectedCharacter;

    // Start is called before the first frame update
    void Start()
    {
        selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
        Debug.Log("START THE GAME WITH THE FISH NUMBER: " + selectedCharacter);
        Switch();
    }


    public void Switch()
    {
        //for (int i = 0; i <= transform.childCount; i++)
        //  transform.GetChild(i).gameObject.SetActive(false);
        Debug.Log("START THE GAME WITH THE FISH NAME: " + transform.GetChild(selectedCharacter-1).gameObject.name);
        transform.GetChild(selectedCharacter-1).gameObject.SetActive(true);
    }
}
