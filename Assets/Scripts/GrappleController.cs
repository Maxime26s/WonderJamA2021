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
                //Debug.DrawLine(transform.position, transform.position + aimDirection, Color.yellow, 1f);
                BeginGrapple();
            } else {
                ///Debug.DrawLine(transform.position, transform.position + aimDirection, Color.red, 1f);
                EndGrapple();
            }
        }
    }

    private void LateUpdate() {
        DrawRope();
    }

    void BeginGrapple() {
        RaycastHit hit;
        //if (Physics.Raycast(transform.position, new Vector3(0, 1, 0), out hit, maxDistance)) {
        if (FanShappedRayCast(transform.position, Vector3.up, out hit, maxDistance, 100, 20)) {
            grapplePoint = hit.point;
            joint = gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(transform.position, grapplePoint);

            joint.maxDistance = maxDistance / 25;
            joint.minDistance = maxDistance / 25;

            //edit values to change gameplay
            joint.spring = 10f;
            joint.damper = 0.5f;
            joint.massScale = 1f;

            lr.positionCount = 2;
        }
    }

    bool FanShappedRayCast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int numberOfRaycast, float arcAngle) {
        hitInfo = new RaycastHit();

        float anglePerLeftCast = arcAngle / numberOfRaycast * Mathf.Deg2Rad;
        float anglePerRightCast = -arcAngle / numberOfRaycast * Mathf.Deg2Rad;
        float ninetyDegreeAngle = 90 * Mathf.Deg2Rad;

        float currentAngle = Mathf.Atan2(direction.x, direction.y);
        float baseAngle = currentAngle + ninetyDegreeAngle;

        Debug.DrawRay(origin, Vector3.up * 50, Color.cyan, 15f);
        if (Physics.Raycast(origin, direction, out hitInfo, maxDistance))
            return true;

        for (int x = 1; x < numberOfRaycast; x++) {
            float leftIterationAngle = baseAngle + anglePerLeftCast * x;
            float rightIterationAngle = baseAngle + anglePerRightCast * x;
            Vector3 iterationDirectionLeft = new Vector3(Mathf.Cos(leftIterationAngle), Mathf.Sin(leftIterationAngle));
            Vector3 iterationDirectionRight = new Vector3(Mathf.Cos(rightIterationAngle), Mathf.Sin(rightIterationAngle));

            Debug.DrawRay(origin, iterationDirectionLeft * 50, Color.green, 15f);
            Debug.DrawRay(origin, iterationDirectionRight * 50, Color.red, 15f);

            if (Physics.Raycast(origin, iterationDirectionLeft, out hitInfo, maxDistance))
                return true;
            if (Physics.Raycast(origin, iterationDirectionRight, out hitInfo, maxDistance))
                return true;
        }
        return false;
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

    void SilenceGrapple() {
        //make the grapple disabled
    }
}
