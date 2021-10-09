using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleController : MonoBehaviour {
    public LineRenderer lr;

    public float maxDistance = 100;
    private Vector3 aimDirection;

    private Vector3 grapplePoint;
    private SpringJoint joint;
    private bool isGrappling = false;


    private void Start() {
        aimDirection = new Vector3(0, maxDistance, 0);
    }
    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            isGrappling = !isGrappling;
            if (!joint) {
                Debug.DrawLine(transform.position, transform.position + aimDirection, Color.yellow, 1f);
                BeginGrapple();
            } else {
                Debug.DrawLine(transform.position, transform.position + aimDirection, Color.red, 1f);
                EndGrapple();
            }
        }
    }

    private void LateUpdate() {
        DrawRope();
    }

    void BeginGrapple() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, new Vector3(0, 1, 0), out hit, maxDistance)) {
            grapplePoint = hit.point;
            joint = gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(transform.position, grapplePoint);

            joint.maxDistance = maxDistance / 5;
            joint.minDistance = maxDistance / 20;

            //edit values to change gameplay
            joint.spring = 5f;
            joint.damper = 0.5f;
            joint.massScale = 3f;

            lr.positionCount = 2;
        }
    }

    void EndGrapple() {
        isGrappling = false;
        lr.positionCount = 0;
        Destroy(joint);
    }
    void DrawRope() {
        if (joint) {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, grapplePoint);
        }
    }
}
