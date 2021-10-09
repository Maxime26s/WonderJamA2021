using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {
    [Header("References")]
    public Rigidbody rigidbody = null;

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


    // Update is called once per frame
    private void Start() {
        Physics.gravity = new Vector3(0, -9.8f * gravityMultiplier, 0);
    }

    private void ManageInputs() {
        if (Input.GetKey(KeyCode.A) && !OverMaxVelocity(Direction.Left)) {
            rigidbody.AddForce(-speed * Time.deltaTime, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.D) && !OverMaxVelocity(Direction.Right)) {
            rigidbody.AddForce(speed * Time.deltaTime, 0f, 0f);
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            HandleJump();
        }
    }

    private void CheckGrounded() {
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

    private bool OverMaxVelocity(Direction direction) {
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
}
