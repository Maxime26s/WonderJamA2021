using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out CharacterController characterController))
        {
            if (characterController.currentState == PlayerState.Grappling)
                characterController.gameObject.GetComponent<GrappleController>().EndGrapple();
            characterController.currentState = PlayerState.OnGround;
            characterController.LoseMoveToPachinko();
        }
        else if (collision.gameObject.TryGetComponent(out Rigidbody rb))
            Destroy(collision.gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CharacterController characterController))
        {
            if (characterController.currentState == PlayerState.Grappling)
                characterController.gameObject.GetComponent<GrappleController>().EndGrapple();
            characterController.currentState = PlayerState.OnGround;
            characterController.LoseMoveToPachinko();
        }
        else if (other.gameObject.TryGetComponent(out Rigidbody rb))
            Destroy(other.gameObject);
    }
}
