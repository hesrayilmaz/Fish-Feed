using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HookController : MonoBehaviour
{
    private Vector3 hookPos, targetPos;
    // Start is called before the first frame update
    void Start()
    {
        hookPos = transform.position;
        targetPos = hookPos - new Vector3(0, 30f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.DOMove(targetPos, 17f);
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}
