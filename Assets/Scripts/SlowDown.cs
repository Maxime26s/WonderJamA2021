using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDown : MonoBehaviour
{
    public float slowDownRatio = 2;
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CharacterController>().speed = 2500f;
            other.GetComponent<CharacterController>().jumpHeight = 250f;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CharacterController>().speed = 5000f;
            other.GetComponent<CharacterController>().jumpHeight = 500f;
        }
    }
}
