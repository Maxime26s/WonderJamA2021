using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;


public enum PlayerState { OnGround, InAir, Grappling, Ragdoll, Pachinker }

public class CharacterController : MonoBehaviour
{
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
    public bool disableJump = false;

    [Header("Ground Checking")]
    public LayerMask groundRaycastLayerMask = new LayerMask();
    public float groundCheckLength = 1.1f;
    public int groundRayCount = 5;
    public Ray groundRay = new Ray();

    [Header("InAir")]
    public float airSpeed = 1f;
    public float maxAirVelocity = 4f;

    [Header("Grappling")]
    public float grapplingSpeed = 5000f;
    public float climbSpeed = 50f;


    [Header("FXs")]
    public ParticleSystem walkParticles = null;
    public ParticleSystem jumpParticles = null;

    [Header("Components")]
    public GrappleController grappleController;
    public ThrowRocksController throwRocksController;

    [Header("StateMachine")]
    public PlayerState currentState = PlayerState.OnGround;
    public bool alive = true;

    private enum Direction { Left, Right };

    private float desiredHorizontalDirection;
    private float desiredVerticalDirection;

    public GameObject meshObject;


    public void SetState(PlayerState newState)
    {
        currentState = newState;

        if (currentState == PlayerState.InAir || currentState == PlayerState.OnGround) {
            meshObject.transform.up = Vector3.up;
        }
    }

    public PlayerState GetState()
    {
        return currentState;
    }

    private void ManageInputs()
    {
        if (GetState() == PlayerState.OnGround || GetState() == PlayerState.Pachinker)
        {
            if (desiredHorizontalDirection < 0 && !OverMaxVelocity(Direction.Left))
            {
                rigidbody.AddForce(desiredHorizontalDirection * speed * Time.deltaTime, 0f, 0f);
            }
            if (desiredHorizontalDirection > 0 && !OverMaxVelocity(Direction.Right))
            {
                rigidbody.AddForce(desiredHorizontalDirection * speed * Time.deltaTime, 0f, 0f);
            }
        } else if (GetState() == PlayerState.InAir) {
            if (desiredHorizontalDirection < 0 && !OverMaxAirVelocity(Direction.Left)) {
                rigidbody.AddForce(desiredHorizontalDirection * airSpeed * Time.deltaTime, 0f, 0f);
            }
            if (desiredHorizontalDirection > 0 && !OverMaxAirVelocity(Direction.Right))
            {
                rigidbody.AddForce(desiredHorizontalDirection * airSpeed * Time.deltaTime, 0f, 0f);
            }
        } else if (GetState() == PlayerState.Grappling) {
            Vector3 direction = grappleController.joint.connectedAnchor - transform.position;
            Vector3 dirLeft = new Vector3(-direction.y, direction.x).normalized;

            if (desiredHorizontalDirection < 0) {
                rigidbody.AddForce(dirLeft * grapplingSpeed * Time.deltaTime);
                Debug.DrawRay(transform.position, dirLeft * grapplingSpeed);
            }
            if (desiredHorizontalDirection > 0)
            {
                rigidbody.AddForce(-dirLeft * grapplingSpeed * Time.deltaTime);
                Debug.DrawRay(transform.position, -dirLeft * grapplingSpeed);

            }
            gameObject.GetComponent<GrappleController>().ChangeDistance(-desiredVerticalDirection * Time.deltaTime * climbSpeed);
        }
    }

    public void OnMove(InputValue input) {
        desiredHorizontalDirection = input.Get<Vector2>().x;
        desiredVerticalDirection = input.Get<Vector2>().y;
        if (currentState != PlayerState.Ragdoll)
            OrientPlayerAccordingToRotation();
    }

    void OrientPlayerAccordingToRotation() {
        if (desiredHorizontalDirection < 0) {
            meshObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        } else if (desiredHorizontalDirection > 0) {
            meshObject.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }

    public void OnJump() {
        if (!disableJump) {
            HandleJump();
        }
    }

    private void CheckGrounded()
    {

        if (GetState() == PlayerState.Grappling)
            return;

        for (int i = 0; i < groundRayCount; i++) {
            Vector3 raypos = transform.position + new Vector3(-(transform.localScale.x / 2f) + i * (transform.localScale.x / (groundRayCount - 1)), 0f, 0f);
            Debug.DrawRay(raypos, Vector3.down * groundCheckLength, Color.red);
            groundRay = new Ray(raypos, Vector3.down);
            if (Physics.Raycast(groundRay, groundCheckLength, groundRaycastLayerMask))
            {
                SetState(PlayerState.OnGround);
                return;
            }
            SetState(PlayerState.InAir);
        }
    }

    private void HandleJump()
    {
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

    void CheckWalkFX()
    {
        if (GetState() == PlayerState.OnGround)
        {
            var em = walkParticles.emission;
            em.enabled = true;
        } else {
            var em = walkParticles.emission;
            em.enabled = false;
        }
    }

    void PlayJumpFX() {
        jumpParticles.Play();
    }

    void Update() {
        if (currentState == PlayerState.OnGround || currentState == PlayerState.Pachinker)
            ApplyGroundFriction();

        if (currentState == PlayerState.OnGround)
            AnimateWalking();

        if (currentState == PlayerState.Grappling)
            AngleSwingingCharacter();

        ManageInputs();
        CheckWalkFX();
        if (currentState != PlayerState.Pachinker) {
            CheckGrounded();
        }
    }

    private void AnimateWalking() {
        if (Mathf.Abs(rigidbody.velocity.x) > 0.1f && currentState == PlayerState.OnGround)
            meshObject.transform.Rotate(Mathf.Sin(Time.time * 10f) / 10f, 0, 0);
    }

    private void AngleSwingingCharacter() {
        Quaternion lookRotation;
        Vector3 direction;
        float turnSpeed = 1f;

        //find the vector pointing from our position to the target
        direction = (grappleController.joint.connectedAnchor - transform.position).normalized;

        //create the rotation we need to be in to look at the target
        meshObject.transform.up = direction;
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

    public void MoveToPachinko()
    {
        for (int i = 0; i < GameManager.Instance.tgm.players.Count; i++)
        {
            if (gameObject == GameManager.Instance.tgm.players[i])
            {
                GameManager.Instance.tgm.targetGroup.m_Targets[i].weight = 0;
                break;
            }
        }
        GameManager.Instance.tgm.players.Remove(gameObject);
        throwRocksController.enabled = true;
        throwRocksController.disableThrowing = false;
        grappleController.enabled = false;
        disableJump = true;
        rigidbody.velocity = Vector3.zero;
        transform.SetParent(Camera.main.gameObject.transform);
        transform.position = GameManager.Instance.pachinkoSawnPoint.transform.position;
        currentState = PlayerState.Pachinker;
        PlayerManager.Instance.livingPlayers.Remove(gameObject);
        PlayerManager.Instance.deadPlayers.Add(gameObject);
        transform.position = GameManager.Instance.pachinkoSawnPoint.transform.position;
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        GetComponent<Collider>().enabled = false;
    }

    public void WinMoveToPachinko()
    {
        PlayerManager.Instance.deadPlayers.Add(gameObject);
        for (int i = 0; i < GameManager.Instance.tgm.players.Count; i++)
        {
            if (gameObject == GameManager.Instance.tgm.players[i])
            {
                GameManager.Instance.tgm.targetGroup.m_Targets[i].weight = 0;
                break;
            }
        }
        GameManager.Instance.tgm.players.Remove(gameObject);
        throwRocksController.enabled = true;
        throwRocksController.disableThrowing = false;
        grappleController.enabled = false;
        disableJump = true;
        rigidbody.velocity = Vector3.zero;
        transform.SetParent(Camera.main.gameObject.transform);
        transform.position = GameManager.Instance.pachinkoSawnPoint.transform.position;
        currentState = PlayerState.Pachinker;
        PlayerManager.Instance.livingPlayers.Remove(gameObject);
        PlayerManager.Instance.wonPlayers.Add(gameObject);
        transform.position = GameManager.Instance.pachinkoSawnPoint.transform.position;
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        GetComponent<Collider>().enabled = false;
    }

    public void MoveToClimbing() {
        throwRocksController.enabled = false;
        throwRocksController.disableThrowing = true;
        grappleController.enabled = true;
        disableJump = false;
        rigidbody.velocity = Vector3.zero;
        transform.SetParent(null);
        PlayerManager.Instance.livingPlayers.Add(gameObject);
        PlayerManager.Instance.deadPlayers.Remove(gameObject);
        transform.localScale = new Vector3(1f, 1f, 1f);
        rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        GetComponent<Collider>().enabled = true;
    }

    private bool OverMaxAirVelocity(Direction direction) {
        if (direction == Direction.Left) {
            if (rigidbody.velocity.x <= -maxAirVelocity) {
                return true;
            }
        }
        else
        {
            if (rigidbody.velocity.x >= maxAirVelocity)
            {
                return true;
            }
        }
        return false;
    }
}
