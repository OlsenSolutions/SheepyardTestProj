using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    void Start()
    {
        
    }
    void Update()
    {
        transform.Rotate(0,2,0);
    }
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<PlayerBehaviour>().tempValue += 10;
        Destroy(this.gameObject);
    }
}
