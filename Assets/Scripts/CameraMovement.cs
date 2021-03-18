using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -5);
    }

    void StopCamera()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

    }
    void OnEnable()
    {
        EventManager.StartListening(EventManager.HitBarrier, StopCamera);

    }

    void OnDisable()
    {
        EventManager.StopListening(EventManager.HitBarrier, StopCamera);


    }
}

