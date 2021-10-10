using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDown : MonoBehaviour
{
    public float speed;
    public float jump;


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CharacterController>().speed = speed;
            other.GetComponent<CharacterController>().jumpHeight =jump;
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
