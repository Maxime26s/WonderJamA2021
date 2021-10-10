using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out CharacterController characterController))
            characterController.MoveToPachinko();
        else if (collision.gameObject.TryGetComponent(out Rigidbody rb))
            Destroy(collision.gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CharacterController characterController))
            characterController.MoveToPachinko();
        else if (other.gameObject.TryGetComponent(out Rigidbody rb))
            Destroy(other.gameObject);
    }
}
