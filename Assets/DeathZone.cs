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
            characterController.MoveToPachinko();
        }
            
        else if (collision.gameObject.TryGetComponent(out Rigidbody rb))
        {
            Destroy(collision.gameObject);
        }
        GameManager.Instance.IsLevelEnd();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CharacterController characterController))
            characterController.MoveToPachinko();
        else if (other.gameObject.TryGetComponent(out Rigidbody rb))
            Destroy(other.gameObject);
    }
}
