using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] private Transform totalEnv, env1, env2;
    private bool isFirstEnv = true;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boundary")
            FixEnvironment();
    }

    void FixEnvironment()
    {
        if (isFirstEnv)
            env1.position = env1.position + new Vector3(0, 0, 600);

        else
            env2.position = env2.position + new Vector3(0, 0, 600);

        isFirstEnv = !isFirstEnv;
    }
}
