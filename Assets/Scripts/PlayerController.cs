using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CoinManager coin;
    [SerializeField] private ScoreManager score;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TileManager tileManager;
    [SerializeField] private float laneDistance = 8;  //distance between two lanes
    

    private int desiredLane = 1;  //0:left, 1:middle, 2:right
    private float forwardSpeed = 0.1f;
    private bool isSpeedUp = false;
    private bool isStarted = true;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isStarted)
        {
            animator = transform.GetComponentsInChildren<Transform>()[1].gameObject.GetComponentsInChildren<Animator>()[0];
            isStarted = false;
        }

        if (gameManager.isPlaying)
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

            if (transform.position.z >= tileManager.GetDestroyPoint())
            {
                forwardSpeed -= 0.002f;
            }

            transform.DOMoveZ(1f, forwardSpeed).SetRelative();

            Vector3 targetPos = transform.position.z * transform.forward + transform.position.y * transform.up;
            if (desiredLane == 0)
                targetPos += Vector3.left * laneDistance;
            else if (desiredLane == 2)
                targetPos += Vector3.right * laneDistance;

            transform.DOMove(targetPos, 0.8f);
        }
        
    }


    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.tag == "Steak")
       {
            coin.IncreaseCoin(15);
            score.IncreaseScore();
            Destroy(other.gameObject);
            IncreaseSize();
            isSpeedUp = true;
       }
       else if (other.gameObject.tag == "Fish")
       {
            coin.IncreaseCoin(10);
            score.IncreaseScore();
            Destroy(other.gameObject);
            IncreaseSize();
        }
        else if (other.gameObject.tag == "BonusFish")
       {
            coin.IncreaseCoin(15);
            score.IncreaseScore();
            Destroy(other.gameObject);
            IncreaseSize();
       }
       else if (other.gameObject.tag == "Trash")
       {
            coin.DecreaseCoin(10);
            Destroy(other.gameObject);
            DecreaseSize();
        }
       else if (other.gameObject.tag == "Obstacle")
       {
            coin.DecreaseCoin(10);
       }
       else if(other.gameObject.tag == "Shark" || other.gameObject.tag == "Hook")
        {
            Debug.Log("GameOver");
            GameOver();
        }

   }

    public void IncreaseSize()
    {
        transform.DOScale(transform.localScale + new Vector3(0.005f, 0.005f, 0.005f), 0.1f);
    }

    public void DecreaseSize()
    {
        transform.DOScale(transform.localScale - new Vector3(0.005f, 0.005f, 0.005f), 0.1f);
    }

    public void GameOver()
    {
        gameManager.ShowPanel();
    }

    IEnumerator Speed()
    {
        transform.DOMoveZ(1f, 0.01f).SetRelative();
        animator.speed = 4;
        yield return new WaitForSeconds(0.3f);
        animator.speed = 1;
        isSpeedUp = false;
    }

  

}
