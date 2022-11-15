using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraHeartButton : MonoBehaviour
{
    private static bool isStartedNew = true;

    // Start is called before the first frame update
    void Start()
    {
        if (isStartedNew)
        {
            isStartedNew = false;
        }
        else
            gameObject.SetActive(false);
    }

}
