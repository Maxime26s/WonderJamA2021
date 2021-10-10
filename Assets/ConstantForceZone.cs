using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantForceZone : MonoBehaviour
{
    public float power;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        other.attachedRigidbody.AddForce(transform.up * power * Time.deltaTime);
    }
}
