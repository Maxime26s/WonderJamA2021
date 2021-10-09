using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeManager : MonoBehaviour
{
    public GameObject hook;
    public GameObject endSegment;
    // Start is called before the first frame update
    public void Setup(GameObject startPoint, RaycastHit endPoint) {
        //if (startPoint.rigidbody)
        //    endSegment.GetComponent<HingeJoint>().connectedBody = startPoint.rigidbody;

        //transform.position = endPoint.rigidbody.position + Vector3.up * 3;
        //hook.transform.position = endPoint.rigidbody.position;
        //transform.parent = endPoint.transform;
        //else
        //    endSegment.GetComponent<HingeJoint>().connectedAnchor = endPoint.transform.position;
    }
}
