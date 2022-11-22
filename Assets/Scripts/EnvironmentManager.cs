using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] private Transform totalEnv, env1, env2;
    private GameManager gameManager;
    private bool isFirstEnv = true;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boundary")
            FixEnvironment();
    }

    private void Update()
    {
        if(gameManager.isPlaying)
            totalEnv.Translate(Vector3.forward * Time.deltaTime * 10f);    
    }

    void FixEnvironment()
    {
        if (isFirstEnv)
            env1.position = env2.position+new Vector3(0, 0, 625);
        else
            env2.position = env1.position+new Vector3(0, 0, 625);

        isFirstEnv = !isFirstEnv;
    }
}
