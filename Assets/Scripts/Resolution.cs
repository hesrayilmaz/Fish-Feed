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
            Screen.SetResolution((int)(Screen.currentResolution.width / 1.25f), (int)(Screen.currentResolution.height / 1.25f), true);
        isStarted = false;
    }

}
