using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnZone : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("prop"))
        {
            Debug.Log(other.gameObject.name);
            other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x, 
                                                               other.gameObject.transform.position.y, 3f);
            Debug.Log(other.gameObject.transform.position);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("prop"))
        {
            Debug.Log(other.gameObject.name + "pogggg");
            other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x,
                                                               other.gameObject.transform.position.y, 0);
        }
    }
}
