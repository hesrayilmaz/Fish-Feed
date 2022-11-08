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
        Switch();
    }


    public void Switch()
    {
        //for (int i = 0; i <= transform.childCount; i++)
          //  transform.GetChild(i).gameObject.SetActive(false);

        transform.GetChild(selectedCharacter).gameObject.SetActive(true);
    }
}
