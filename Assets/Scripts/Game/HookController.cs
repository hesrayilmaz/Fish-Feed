using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    private Vector3 hookPos, targetPos;
    public static float speed = 4; 

    // Start is called before the first frame update
    void Start()
    {
        hookPos = transform.position;
        targetPos = hookPos - new Vector3(0, 30f, 0);
        speed += 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime*speed);
    }

}
