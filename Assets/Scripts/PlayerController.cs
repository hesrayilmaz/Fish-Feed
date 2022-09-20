using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Score score;
    [SerializeField] private float forwardSpeed;
    private int desiredLane = 1;  //0:left, 1:middle, 2:right
    [SerializeField] private float laneDistance = 8;  //distance between two lanes
    
    [SerializeField] private SimpleAnimancer animancer;
    [SerializeField] private string swimAnimName = "Swim";
    [SerializeField] private float swimAnimSpeed = 3f;
    private bool isSpeedUp = false;

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

        if (isSpeedUp)
        {
            StartCoroutine(Speed());
            
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
       if (other.gameObject.tag == "Steak")
       {
            score.IncreaseScore(15);
            Destroy(other.gameObject);
            transform.DOScale(transform.localScale + new Vector3(0.1f, 0.05f, 0.05f), 0.1f);
            isSpeedUp = true;
       }
       else if (other.gameObject.tag == "Fish")
       {
            score.IncreaseScore(10);
            Destroy(other.gameObject);
            transform.DOScale(transform.localScale + new Vector3(0.1f, 0.05f, 0.05f), 0.1f);
       }
        else if (other.gameObject.tag == "BonusFish")
       {
            score.IncreaseScore(15);
            Destroy(other.gameObject);
            transform.DOScale(transform.localScale + new Vector3(0.1f, 0.05f, 0.05f), 0.1f);
       }
       else if (other.gameObject.tag == "FishBone")
       {
            score.IncreaseScore(5);
            Destroy(other.gameObject);
            transform.DOScale(transform.localScale + new Vector3(0.1f, 0.05f, 0.05f), 0.1f);
       }
       else if (other.gameObject.tag == "Trash")
       {
            score.DecreaseScore(10);
            Destroy(other.gameObject);
            transform.DOScale(transform.localScale - new Vector3(0.1f, 0.05f, 0.05f), 0.1f);
       }
       else if (other.gameObject.tag == "Obstacle")
       {
            score.DecreaseScore(10);
       }
       else if(other.gameObject.tag == "Shark" || other.gameObject.tag == "Hook")
        {
            Debug.Log("GameOver");
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


    IEnumerator Speed()
    {
        transform.DOMoveZ(1f, 0.01f).SetRelative();
        swimAnimSpeed = 8f;
        SwimAnimation();
        yield return new WaitForSeconds(0.3f);
        swimAnimSpeed = 3f;
        SwimAnimation();
        isSpeedUp = false;
    }

}
