using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float forwardSpeed;
    private int desiredLane = 1;  //0:left, 1:middle, 2:right
    [SerializeField] private float laneDistance = 8;  //distance between two lanes
    
    [SerializeField] private SimpleAnimancer animancer;
    [SerializeField] private string swimAnimName = "Swim";
    [SerializeField] private float swimAnimSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        SwimAnimation();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (desiredLane != 2)
                desiredLane++;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (desiredLane != 0)
                desiredLane--;
        }

        transform.DOMoveZ(1f, 0.1f).SetRelative();

        Vector3 targetPos = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (desiredLane == 0)
            targetPos += Vector3.left * laneDistance;
        else if (desiredLane == 2)
            targetPos += Vector3.right * laneDistance;

        transform.DOMove(targetPos, 0.8f);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Food" || other.gameObject.tag == "Steak")
        {
            Destroy(other.gameObject);
        }
        else if(other.gameObject.tag == "Trash")
        {
            Destroy(other.gameObject);
        }


    }



    public void SwimAnimation()
    {
        PlayAnimation(swimAnimName, swimAnimSpeed);
    }
    public void PlayAnimation(string animName, float animSpeed)
    {
        animancer.PlayAnimation(animName);
        animancer.SetStateSpeed(animSpeed);
    }

}
