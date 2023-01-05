using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CoinManager coin;
    [SerializeField] private ScoreManager score;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TileManager tileManager;
    [SerializeField] private AudioSource bubbleAudio;
    [SerializeField] private AudioSource wrongItemAudio;
    [SerializeField] private AudioSource gameOverAudio;
    [SerializeField] private GameObject wrongItemParticle, correctItemParticle;
    [SerializeField] private GameObject scorePopup;

    [SerializeField] private float laneDistance = 8;  //distance between two lanes
    [SerializeField] private float forwardSpeed;

    private int desiredLane = 1;  //0:left, 1:middle, 2:right
    private bool isSpeedUp = false;
    private bool isStarted = true;
    private Vector3 targetPos;
    private Animator animator;


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

            if (Input.GetKeyDown(KeyCode.RightArrow) || SwipeManager.swipeRight)
            {
                if (desiredLane != 2)
                    desiredLane++;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || SwipeManager.swipeLeft)
            {
                if (desiredLane != 0)
                    desiredLane--;
            }

            if (isSpeedUp)
            {
                StartCoroutine(Speed());
            }

            if (transform.position.z >= tileManager.GetDestroyPoint()-0.1f)
            {
                forwardSpeed += 3f;
                Debug.Log("INCREASE SPEED");
            }
           
            transform.Translate(Vector3.forward * Time.deltaTime * forwardSpeed);
            
            targetPos = transform.position.z * transform.forward + transform.position.y * transform.up;
            if (desiredLane == 0)
                targetPos += Vector3.left * laneDistance;
            else if (desiredLane == 2)
                targetPos += Vector3.right * laneDistance;

            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 10);
        }
        
    }


    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.tag == "Steak")
       {
            TriggerCorrectItem(other.gameObject, 15);
            isSpeedUp = true;
       }
       else if (other.gameObject.tag == "Fish")
       {
            TriggerCorrectItem(other.gameObject, 10);
       }
       else if (other.gameObject.tag == "BonusFish")
       {
            TriggerCorrectItem(other.gameObject, 15);
       }
       else if (other.gameObject.tag == "Trash")
       {
            TriggerWrongItem(other.gameObject, 10);
       }
       else if (other.gameObject.tag == "Obstacle" || other.gameObject.tag == "Shark" || other.gameObject.tag == "Hook")
       {
            Debug.Log("GameOver");
            GameOver();
       }
       
   }

    public void GameOver()
    {
        gameManager.ShowPanel();
        gameOverAudio.Play();
    }

    private void TriggerCorrectItem(GameObject triggerObject, int scoreAmount)
    {
        coin.IncreaseCoin(scoreAmount);
        score.IncreaseScore();
        Destroy(triggerObject);
        bubbleAudio.Play();
        GameObject popup = Instantiate(scorePopup, triggerObject.transform.position + new Vector3(1, 0, 1), Quaternion.identity);
        popup.GetComponent<ScorePopup>().Setup("+"+scoreAmount, true);
        StartCoroutine(CreateParticle(correctItemParticle, triggerObject));
    }

    private void TriggerWrongItem(GameObject triggerObject, int scoreAmount)
    {
        coin.DecreaseCoin(scoreAmount);
        Destroy(triggerObject);
        wrongItemAudio.Play();
        GameObject popup = Instantiate(scorePopup, triggerObject.transform.position + new Vector3(1, 0, 1), Quaternion.identity);
        popup.GetComponent<ScorePopup>().Setup("-" + scoreAmount, false);
        StartCoroutine(CreateParticle(wrongItemParticle, triggerObject));
    }

    IEnumerator Speed()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * 130);
        animator.speed = 3;
        yield return new WaitForSeconds(0.3f);
        animator.speed = 1f;
        isSpeedUp = false;
    }
    
    IEnumerator CreateParticle(GameObject particle, GameObject triggerObject)
    {
        GameObject go = Instantiate(particle, triggerObject.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Destroy(go);
    }

    public float GetSpeed()
   {
        return forwardSpeed;
   }

}
