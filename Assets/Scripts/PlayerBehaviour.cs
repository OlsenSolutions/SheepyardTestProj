using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    private bool isChangingTrack = false;
    private bool onGround = true;
    public int tempValue;
    int score;
    public Text ScoreValue;
    void Start()
    {
        tempValue = 0;
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -5);
    }

    void Update()
    {
        if (Input.GetKey("a") && !isChangingTrack && transform.position.x < 0.9&& onGround)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(2, 0, -5);
            isChangingTrack = true;
            StartCoroutine(stopOnTrack());
        }

        if (Input.GetKey("d") && !isChangingTrack && transform.position.x  > -0.9 && onGround)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(-2, 0, -5);
            isChangingTrack = true;
            StartCoroutine(stopOnTrack());

        }

        if (Input.GetKey("space") && !isChangingTrack && onGround)
        {
            onGround = false;
            GetComponent<Animator>().SetTrigger("Jump");
            GetComponent<Rigidbody>().velocity = new Vector3(0, 2.5f, -5);
            StartCoroutine(stopJumping());

        }
        
        score = tempValue + (int)(-1 * (transform.position.z - 1));
        ScoreValue.text = "Score:" + score.ToString();

    }
    IEnumerator stopJumping()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<Rigidbody>().velocity = new Vector3(0, -2.5f, -5);
        yield return new WaitForSeconds(0.5f);
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -5);
        onGround = true;
    }
    IEnumerator stopOnTrack()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -5);
        isChangingTrack = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (onGround)
        {

            Dead();
        }
        else
        {
            while (onGround)
            {
                Wait();
                Dead();
            }
    }

        void Dead()
        {
            StopAllCoroutines();
            EventManager.TriggerEvent(EventManager.HitBarrier);
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Animator>().SetTrigger("Death");
            EventManager.TriggerEvent(EventManager.EndGame);
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
    }
}
