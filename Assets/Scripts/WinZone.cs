using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out CharacterController characterController))
        {
            characterController.WinMoveToPachinko();
            GameManager.Instance.IsLevelEnd();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CharacterController characterController))
        {
            characterController.WinMoveToPachinko();
            GameManager.Instance.IsLevelEnd();
        }
    }
}
