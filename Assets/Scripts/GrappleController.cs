using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class GrappleController : MonoBehaviour {
    public LineRenderer lr;

    public float maxDistance = 100;
    private Vector3 aimDirection;

    private Vector3 grapplePoint;
    private SpringJoint joint;
    public bool isGrappling = false;


    private void Start() {
        aimDirection = new Vector3(0, maxDistance, 0);
    }
    // Update is called once per frame
    void Update() {
    }

    private void OnGrapple() {
        if (!joint && !gameObject.GetComponent<CharacterController>().grounded) {
            //Debug.DrawLine(transform.position, transform.position + aimDirection, Color.yellow, 1f);
            BeginGrapple();
        } else {
            ///Debug.DrawLine(transform.position, transform.position + aimDirection, Color.red, 1f);
            EndGrapple();
        }
    }
    public void OnAim(InputValue input) {
        //Debug.DrawRay(transform.position, new Vector3(input.Get<Vector2>().x, input.Get<Vector2>().y, 0), Color.blue, 0.2f);
        Vector3 inputDirection = new Vector3(input.Get<Vector2>().x, input.Get<Vector2>().y, 0);
        aimDirection = inputDirection;
    }

    private void LateUpdate() {
        DrawRope();
    }

    void BeginGrapple() {
        isGrappling = true;
        RaycastHit hit;
        //if (Physics.Raycast(transform.position, new Vector3(0, 1, 0), out hit, maxDistance)) {
        if (FanShappedRayCast(transform.position, aimDirection, out hit, maxDistance, 100, 20)) {
            grapplePoint = hit.point;
            joint = gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(transform.position, grapplePoint);

            joint.maxDistance = maxDistance / 25;
            joint.minDistance = maxDistance / 25;

            //edit values to change gameplay
            joint.spring = 8f;
            joint.damper = 0.5f;
            joint.massScale = 1f;

            lr.positionCount = 2;
        }
    }

    bool FanShappedRayCast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int numberOfRaycast, float arcAngle) {
        hitInfo = new RaycastHit();

        //Debug.DrawRay(origin, direction * 50, Color.cyan, 5f);
        if (Physics.Raycast(origin, direction, out hitInfo, maxDistance))
            return true;

        direction = new Vector3(direction.x, -direction.y, direction.z);
        float anglePerLeftCast = arcAngle / numberOfRaycast * Mathf.Deg2Rad;
        float anglePerRightCast = -arcAngle / numberOfRaycast * Mathf.Deg2Rad;
        float ninetyDegreeAngle = 90 * Mathf.Deg2Rad;

        float currentAngle = Mathf.Atan2(direction.x, direction.y);
        float baseAngle = currentAngle - ninetyDegreeAngle;


        for (int x = 1; x < numberOfRaycast; x++) {
            float leftIterationAngle = baseAngle + anglePerLeftCast * x;
            float rightIterationAngle = baseAngle + anglePerRightCast * x;
            Vector3 iterationDirectionLeft = new Vector3(Mathf.Cos(leftIterationAngle), Mathf.Sin(leftIterationAngle));
            Vector3 iterationDirectionRight = new Vector3(Mathf.Cos(rightIterationAngle), Mathf.Sin(rightIterationAngle));

            //Debug.DrawRay(origin, iterationDirectionLeft * 50, Color.green, 5f);
            //Debug.DrawRay(origin, iterationDirectionRight * 50, Color.red, 5f);

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

    public void SilenceGrapple(float silenceDuration) {
        //make the grapple disabled
    }
}
