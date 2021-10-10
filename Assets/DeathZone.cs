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
        {
            if (characterController.currentState == PlayerState.Grappling)
                characterController.gameObject.GetComponent<GrappleController>().EndGrapple();
            characterController.currentState = PlayerState.OnGround;
            characterController.MoveToPachinko();
        }
            
        else if (collision.gameObject.TryGetComponent(out Rigidbody rb))
            Destroy(collision.gameObject);
    }
}
