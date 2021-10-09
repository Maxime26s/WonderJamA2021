using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;


public enum PlayerState { OnGround, InAir, Grappling, Ragdoll, Pachinker }

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
    public int groundRayCount = 5;
    public Ray groundRay = new Ray();

    [Header("FXs")]
    public ParticleSystem walkParticles = null;
    public ParticleSystem jumpParticles = null;

    [Header("StateMachine")]
    public PlayerState currentState = PlayerState.OnGround;

    private enum Direction { Left, Right };

    private float desiredHorizontalDirection;
    private float desiredVerticalDirection;


    public void SetState(PlayerState newState) {
        currentState = newState;
    }

    public PlayerState GetState() {
        return currentState;
    }

    private void ManageInputs() {
        if (desiredHorizontalDirection < 0 && !OverMaxVelocity(Direction.Left)) {
            rigidbody.AddForce(desiredHorizontalDirection * speed * Time.deltaTime, 0f, 0f);
        }
        if (desiredHorizontalDirection > 0 && !OverMaxVelocity(Direction.Right)) {
            rigidbody.AddForce(desiredHorizontalDirection * speed * Time.deltaTime, 0f, 0f);
        }
    }

    public void OnMove(InputValue input) {
        desiredHorizontalDirection = input.Get<Vector2>().x;
        desiredVerticalDirection = input.Get<Vector2>().y;
    }

    public void OnJump() {
        HandleJump();
    }

    private void CheckGrounded() {

        if (GetState() == PlayerState.Grappling)
            return;

        for (int i = 0; i < groundRayCount; i++) {
            Vector3 raypos = transform.position + new Vector3(-(transform.localScale.x/2f) + i*(transform.localScale.x/(groundRayCount - 1)),0f,0f);
            Debug.DrawRay(raypos, Vector3.down * groundCheckLength, Color.red);
            groundRay = new Ray(raypos, Vector3.down);
            if (Physics.Raycast(groundRay, groundCheckLength, groundRaycastLayerMask)) {
                SetState(PlayerState.OnGround);
                return;
            }
            SetState(PlayerState.InAir);
        }
    }

    private void HandleJump() {
        if (currentState != PlayerState.OnGround)
            return;
        if (resetVelocityOnJump)
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, 0f);
        rigidbody.AddForce(0f, jumpHeight, 0f);
        PlayJumpFX();
        SetState(PlayerState.InAir);
    }

    private void ApplyGroundFriction() {
        if (rigidbody.velocity.x > 0) { // Going Right
            float verification = rigidbody.velocity.x - groundFriction * Time.deltaTime;
            if (verification < 0)
                rigidbody.velocity -= new Vector3(rigidbody.velocity.x, 0, 0);
            else
                rigidbody.velocity -= new Vector3(groundFriction, 0, 0) * Time.deltaTime;
        } else {                                        // Going Left
            float verification = rigidbody.velocity.x + groundFriction * Time.deltaTime;
            if (verification > 0)
                rigidbody.velocity -= new Vector3(rigidbody.velocity.x, 0, 0);
            else
                rigidbody.velocity += new Vector3(groundFriction, 0, 0) * Time.deltaTime;
        }
    }

    void CheckWalkFX() {
        if (GetState() == PlayerState.OnGround) {
            var em = walkParticles.emission;
            em.enabled = true;
        }
        else {
            var em = walkParticles.emission;
            em.enabled = false;
        }
    }

    void PlayJumpFX() {
        jumpParticles.Play();
    }

    void Update() {
        if (currentState == PlayerState.OnGround) {
            ManageInputs();
            ApplyGroundFriction();
        }
        CheckWalkFX();
        CheckGrounded();

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
