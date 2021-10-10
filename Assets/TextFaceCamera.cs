using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFaceCamera : MonoBehaviour
{

    public Transform playerTransform;
    public float height;

    // Update is called once per frame
    void Update()
    {
        transform.forward = transform.position - Camera.main.transform.position;
        transform.position = playerTransform.position + Vector3.up * height;
    }
}
