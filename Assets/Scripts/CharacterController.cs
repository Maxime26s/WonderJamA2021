using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class CharacterController : MonoBehaviour {
    [Header("References")]
    public new Rigidbody rigidbody = null;

    [Header("Horizontal Speed")]
    public float speed = 2f;
    public float maxVelocity = 2f;
    public float groundFriction = 1f;

    [Header("Vertical Speed")]
    public float jumpHeight = 10f;
    public float gravityMultiplier = 4f;
    public bool resetVelocityOnJump = false;

    [Header("Ground Checking")]
    public LayerMask groundRaycastLayerMask = new LayerMask();
    public float groundCheckLength = 1.1f;
    public bool grounded = false;
    public int groundRayCount = 5;
    public Ray groundRay = new Ray();

    private enum Direction { Left, Right };
    private float desiredHorizontalDirection;
    private float desiredVerticalDirection;

    // Update is called once per frame
    private void Start() {
        Physics.gravity = new Vector3(0, -9.8f * gravityMultiplier, 0);
    }

    private void ManageInputs() {
        float grappleMult = 1f;
        if (GetComponent<GrappleController>().isGrappling)
            grappleMult = 0.5f;

        if (desiredHorizontalDirection < 0 && !OverMaxUngrappledVelocity(Direction.Left) || GetComponent<GrappleController>().isGrappling) {
            rigidbody.AddForce(desiredHorizontalDirection * speed * Time.deltaTime * grappleMult, 0f, 0f);
        }
        if (desiredHorizontalDirection > 0 && (!OverMaxUngrappledVelocity(Direction.Right) || GetComponent<GrappleController>().isGrappling)) {
            rigidbody.AddForce(desiredHorizontalDirection * speed * Time.deltaTime * grappleMult, 0f, 0f);
        }
        rigidbody.AddForce(0f, desiredVerticalDirection * 200 * Time.deltaTime, 0f);
        rigidbody.AddForce(Vector3.down * 30 * Time.deltaTime);
    }

    public void OnMove(InputValue input) {
        desiredHorizontalDirection = input.Get<Vector2>().x;
        desiredVerticalDirection = input.Get<Vector2>().y;
    }

    public void OnJump() {
        //if (Input.GetKeyDown(KeyCode.Space)) {
            HandleJump();
        //}
    }

    private void CheckGrounded() {
        if (gameObject.GetComponent<GrappleController>().isGrappling) {
            grounded = false;
            return;
        }

        for (int i = 0; i < groundRayCount; i++) {
            Vector3 raypos = transform.position + new Vector3(-(transform.localScale.x/2f) + i*(transform.localScale.x/(groundRayCount - 1)),0f,0f);
            Debug.DrawRay(raypos, Vector3.down * groundCheckLength, Color.red);
            groundRay = new Ray(raypos, Vector3.down);
            if (Physics.Raycast(groundRay, groundCheckLength, groundRaycastLayerMask)) {
                grounded = true;
                return;
            }
        }

        grounded = false;
    }


    private void HandleJump() {
        if (!grounded)
            return;
        if (resetVelocityOnJump)
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, 0f);
        rigidbody.AddForce(0f, jumpHeight, 0f);
        grounded = false;
    }

    private void ApplyGroundFriction() {
        if (rigidbody.velocity.x > 0) { // Going Right
            float verification = rigidbody.velocity.x - groundFriction * Time.deltaTime;
            if (verification < 0)
                rigidbody.velocity -= new Vector3(rigidbody.velocity.x, 0, 0);
            else
                rigidbody.velocity -= new Vector3(groundFriction, 0, 0) * Time.deltaTime;
        } else {                          // Going Left
            float verification = rigidbody.velocity.x + groundFriction * Time.deltaTime;
            if (verification > 0)
                rigidbody.velocity -= new Vector3(rigidbody.velocity.x, 0, 0);
            else
                rigidbody.velocity += new Vector3(groundFriction, 0, 0) * Time.deltaTime;
        }
    }

    void Update() {
        ApplyGroundFriction();
        CheckGrounded();
        ManageInputs();
    }

    private bool OverMaxUngrappledVelocity(Direction direction) {
        if (direction == Direction.Left) {
            if (rigidbody.velocity.x <= -maxVelocity) {
                return true;
            }
        } else {
            if (rigidbody.velocity.x >= maxVelocity) {
                return true;
            }
        }
        return false;
    }

    //private bool capMaxVelocity() {
    //    if (rigidbody.velocity.x > ) {
    //        rigidbody.velocity = maxVelocity * 2;
    //    }
    //}
}
