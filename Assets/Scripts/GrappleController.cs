using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class GrappleController : MonoBehaviour {
    public LineRenderer lr;
    public CharacterController characterController = null;
    public float maxDistance = 100;
    private Vector3 aimDirection;

    private Vector3 grapplePoint;
    public SpringJoint joint;
    public bool canGrapple = true;

    public float spring;
    public float damper;
    public float massScale;

    public GameObject rope;
    public GameObject ropeRef;

    private void Start() {
        aimDirection = new Vector3(0, maxDistance, 0);
    }
    // Update is called once per frame
    void Update() {
    }

    private void OnGrapple() {
        if (characterController.GetState() == PlayerState.Grappling)
            EndGrapple();
        else if (canGrapple && characterController.GetState() == PlayerState.InAir)
            BeginGrapple();
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
        RaycastHit hit;
        //if (Physics.Raycast(transform.position, new Vector3(0, 1, 0), out hit, maxDistance)) {
        if (FanShappedRayCast(transform.position, aimDirection, out hit, maxDistance, 100, 20)) {
            characterController.SetState(PlayerState.Grappling);
            grapplePoint = hit.point;
            //ropeRef = Instantiate(rope);
            //ropeRef.transform.SetParent(gameObject.transform);
            //ropeRef.GetComponent<RopeManager>().Setup(gameObject, hit);
            joint = gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(transform.position, grapplePoint);

            joint.maxDistance = distanceFromPoint / 1.1f;
            joint.minDistance = distanceFromPoint / 1.2f;

            //edit values to change gameplay
            joint.spring = spring;
            joint.damper = damper;
            joint.massScale = massScale;

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

            if (Physics.Raycast(origin, iterationDirectionLeft, out  hitInfo, maxDistance))
                return true;
            if (Physics.Raycast(origin, iterationDirectionRight, out hitInfo, maxDistance))
                return true;
        }
        return false;
    }

    void EndGrapple() {
        characterController.SetState(PlayerState.InAir);
        lr.positionCount = 0;
        Destroy(joint);
        //Destroy(ropeRef);
    }

    void DrawRope() {
        if (joint) {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, grapplePoint);
        }
    }

    public void SilenceGrapple(float time) {
        StartCoroutine("DisableCoroutine", time);
    }

    IEnumerator DisableCoroutine(float time) {
        canGrapple = false;
        EndGrapple();

        yield return new WaitForSeconds(time);

        canGrapple = true;
    }

    public void ChangeDistance(float val) {
        //if joint too small to shrink further
        if (val < 0 && joint.maxDistance < joint.maxDistance / 20f) {
            return;
        }
        //if joint too big to enlarge
        if (val > 0 && joint.maxDistance > maxDistance) {
            return;
        }
        joint.maxDistance += val;
        joint.minDistance += val;
    }
}
