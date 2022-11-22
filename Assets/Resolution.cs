using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resolution : MonoBehaviour
{
    // Start is called before the first frame update
    private static bool isStarted = true;
    void Start()
    {
        if (isStarted)
            Screen.SetResolution(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2, true);
        isStarted = false;
    }

}
