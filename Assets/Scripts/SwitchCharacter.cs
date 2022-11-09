using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCharacter : MonoBehaviour
{
    private string selectedCharacter;

    // Start is called before the first frame update
    void Start()
    {
        selectedCharacter = PlayerPrefs.GetString("selectedCharacter");
        Debug.Log("START THE GAME WITH THE FISH NUMBER: " + selectedCharacter);
        Switch();
    }


    public void Switch()
    {
        //for (int i = 0; i <= transform.childCount; i++)
        //  transform.GetChild(i).gameObject.SetActive(false);
        Debug.Log("START THE GAME WITH THE FISH NAME: " + transform.Find(selectedCharacter).gameObject.name);
        transform.Find(selectedCharacter).gameObject.SetActive(true);
    }
}
