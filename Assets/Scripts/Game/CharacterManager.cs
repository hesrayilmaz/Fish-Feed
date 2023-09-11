using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject[] fishPrefabs;
    [SerializeField] private Transform fishParent;
    private int selectedCharacterIndex;

    // Start is called before the first frame update
    void Start()
    {
        selectedCharacterIndex = PlayerPrefs.GetInt("selectedCharacter", 0);
        SetCharacter();
    }

    public void SetCharacter()
    {
        Instantiate(fishPrefabs[selectedCharacterIndex], fishParent);
    }
}
