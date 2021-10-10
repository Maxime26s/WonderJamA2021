using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out CharacterController characterController))
            characterController.WinMoveToPachinko();
        else if (collision.gameObject.TryGetComponent(out Rigidbody rb))
        {
            Destroy(collision.gameObject);
        }
        GameManager.Instance.IsLevelEnd();
    }
}
