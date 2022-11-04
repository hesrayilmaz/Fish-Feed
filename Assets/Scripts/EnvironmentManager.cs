using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] private Transform totalEnv, env1, env2;
    private bool isFirstEnv = true;
    private float offsetZ;

    // Start is called before the first frame update
    void Start()
    {
        offsetZ = transform.position.z - totalEnv.position.z +200;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boundary")
            FixEnvironment();
    }
    /*void LateUpdate()
    {
        Vector3 newPosition = new Vector3(env.position.x, env.position.y, offsetZ + env.position.z);
        env.transform.position = Vector3.Lerp(env.transform.position, newPosition, 0.1f * Time.deltaTime);
    }*/

    private void Update()
    {
        if(GameManager.instance.isPlaying)
            totalEnv.Translate(Vector3.forward * Time.deltaTime * 15f);    
    }

    void FixEnvironment()
    {
        //totalEnv.transform.position += new Vector3(0, 0, 30);
        if (isFirstEnv)
            env1.position = env2.position+new Vector3(0, 0, 625);
        else
            env2.position = env1.position+new Vector3(0, 0, 625);

        isFirstEnv = !isFirstEnv;
    }
}
