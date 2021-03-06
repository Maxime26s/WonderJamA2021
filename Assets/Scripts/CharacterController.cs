using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;
using TMPro;

public enum PlayerState { OnGround, InAir, Grappling, Ragdoll, Pachinker }

public class CharacterController : MonoBehaviour {
    [Header("References")]
    public new Rigidbody rigidbody = null;

    [Header("Horizontal Speed")]
    public float speed = 5000f;
    public float maxVelocity = 5f;
    public float groundFriction = 30f;

    [Header("Vertical Speed")]
    public float jumpHeight = 500f;
    public float gravityMultiplier = 1f;
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
    public TextMeshProUGUI text;
    public MeshRenderer mrBody;
    public LineRenderer lrRope;
    public int playerId;

    public bool inAirAfterGrappling = false;

    public List<AudioClip> jumpSounds = null;
    public List<AudioClip> grappleSounds = null;
    public AudioSource playerAudioSource = null;

    public void SetState(PlayerState newState) {
        if (currentState == newState || currentState == PlayerState.Ragdoll)
            return;

        if (newState == PlayerState.Ragdoll)
            StartCoroutine("RagdollPlayer", 3f);

        currentState = newState;

        if (currentState == PlayerState.InAir || currentState == PlayerState.OnGround) {
            float yRotation = meshObject.transform.rotation.eulerAngles.y;
            meshObject.transform.up = Vector3.up;
            meshObject.transform.rotation = Quaternion.Euler(0, yRotation, 0);
            meshObject.transform.localScale = new Vector3(Mathf.Abs(meshObject.transform.localScale.x), Mathf.Abs(meshObject.transform.localScale.y), Mathf.Abs(meshObject.transform.localScale.z));
        }
    }



    public void RagdollPlayer(float time) {
        StartCoroutine("RagdollPlayerCoroutine", time);
    }

    internal void SetColor(int nbPlayer, Color32 color32)
    {
        playerId = nbPlayer;
        text.text = "P" + (nbPlayer + 1);
        text.color = color32;
        mrBody.material.color = PlayerManager.Instance.colors[playerId];
        lrRope.material.SetColor("_EmissionColor", PlayerManager.Instance.colors[playerId]);
    }

    private IEnumerator RagdollPlayerCoroutine(float time) {
        grappleController.EndGrapple();

        rigidbody.freezeRotation = false;
        rigidbody.AddTorque(UnityEngine.Random.Range(-50f, 50f), UnityEngine.Random.Range(-50f, 50f), UnityEngine.Random.Range(-50f, 50f));

        yield return new WaitForSeconds(time);

        rigidbody.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        rigidbody.freezeRotation = true;
        currentState = PlayerState.InAir;
    }

    public void BeginAirSpinAfterGrapple() {
        rigidbody.freezeRotation = false;
        rigidbody.AddTorque(0f, 0f, rigidbody.velocity.x * 10);
    }


    internal void SetText(int nbPlayer, Color32 color32) {
        text.text = "P" + (nbPlayer + 1);
        text.color = color32;
    }

    public PlayerState GetState() {
        return currentState;
    }

    private void ManageInputs() {
        if (GetState() == PlayerState.OnGround) {
            if (desiredHorizontalDirection < 0 && !OverMaxVelocity(Direction.Left)) {
                rigidbody.AddForce(desiredHorizontalDirection * speed * Time.deltaTime, 0f, 0f);
            }
            if (desiredHorizontalDirection > 0 && !OverMaxVelocity(Direction.Right)) {
                rigidbody.AddForce(desiredHorizontalDirection * speed * Time.deltaTime, 0f, 0f);
            }
        } else if (GetState() == PlayerState.Pachinker) {
            if (desiredHorizontalDirection < 0 && !OverMaxVelocity(Direction.Left) && transform.localPosition.x > -5) {
                rigidbody.AddForce(desiredHorizontalDirection * speed * Time.deltaTime, 0f, 0f);
            }
            if (desiredHorizontalDirection > 0 && !OverMaxVelocity(Direction.Right) && transform.localPosition.x < 5) {
                rigidbody.AddForce(desiredHorizontalDirection * speed * Time.deltaTime, 0f, 0f);
            }
        } else if (GetState() == PlayerState.InAir) {
            if (desiredHorizontalDirection < 0 && !OverMaxAirVelocity(Direction.Left)) {
                rigidbody.AddForce(desiredHorizontalDirection * airSpeed * Time.deltaTime, 0f, 0f);
            }
            if (desiredHorizontalDirection > 0 && !OverMaxAirVelocity(Direction.Right)) {
                rigidbody.AddForce(desiredHorizontalDirection * airSpeed * Time.deltaTime, 0f, 0f);
            }
        } else if (GetState() == PlayerState.Grappling) {
            Vector3 direction = grappleController.joint.connectedAnchor - transform.position;
            Vector3 dirLeft = new Vector3(-direction.y, direction.x).normalized;

            if (desiredHorizontalDirection < 0) {
                rigidbody.AddForce(dirLeft * grapplingSpeed * Time.deltaTime);
                Debug.DrawRay(transform.position, dirLeft * grapplingSpeed);
            }
            if (desiredHorizontalDirection > 0) {
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
            OrientPlayerAccordingToGroundSpeed();
    }

    void OrientPlayerAccordingToGroundSpeed() {
        if (currentState != PlayerState.Ragdoll /*&& !inAirAfterGrappling*/) {
            if (desiredHorizontalDirection < 0) {
                meshObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            } else if (desiredHorizontalDirection > 0) {
                meshObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    public void OnJump() {
        if (!disableJump) {
            HandleJump();
        }
    }

    public void ResetRotation() {
        //rigidbody.freezeRotation = false;
        //rigidbody.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        //meshObject.GetComponent<Rigidbody>().rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        //OrientPlayerAccordingToGroundSpeed();
        //rigidbody.freezeRotation = true;
        //inAirAfterGrappling = false;
        rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        OrientPlayerAccordingToGroundSpeed();
        transform.eulerAngles = new Vector3(0, 0, 0);
        inAirAfterGrappling = false;

    }
    private void CheckGrounded() {

        if (GetState() == PlayerState.Grappling)
            return;

        for (int i = 0; i < groundRayCount; i++) {
            Vector3 raypos = transform.position + new Vector3(-(transform.localScale.x / 2f) + i * (transform.localScale.x / (groundRayCount - 1)), 0f, 0f);
            Debug.DrawRay(raypos, Vector3.down * groundCheckLength, Color.red);
            groundRay = new Ray(raypos, Vector3.down);
            if (Physics.Raycast(groundRay, groundCheckLength, groundRaycastLayerMask)) {

                if (currentState == PlayerState.InAir/* && inAirAfterGrappling*/) {
                    ResetRotation();
                }
                SetState(PlayerState.OnGround);
                return;
            }
        }
        SetState(PlayerState.InAir);
    }

    private void HandleJump() {
        if (currentState != PlayerState.OnGround)
            return;
        if (resetVelocityOnJump)
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, 0f);

        if (playerAudioSource != null && jumpSounds != null && jumpSounds.Count > 0 && EffectController.Instance != null) {
            playerAudioSource.PlayOneShot(jumpSounds[UnityEngine.Random.Range(0, jumpSounds.Count)]);
        }
        inAirAfterGrappling = false;
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
        } else {
            var em = walkParticles.emission;
            em.enabled = false;
        }
    }

    void PlayJumpFX() {
        jumpParticles.Play();
    }

    void Update() {
        if (currentState == PlayerState.Ragdoll)
            return;

        if (currentState == PlayerState.OnGround || currentState == PlayerState.Pachinker)
            ApplyGroundFriction();

        if (currentState == PlayerState.OnGround) {
            OrientPlayerAccordingToGroundSpeed();
            AnimateWalking();
        }

        if (currentState == PlayerState.Grappling)
            AngleSwingingCharacter();

        ManageInputs();
        CheckWalkFX();
        if (currentState != PlayerState.Pachinker) {
            CheckGrounded();
        }
    }

    private void AnimateWalking() {
        if (currentState == PlayerState.OnGround/* && !inAirAfterGrappling*/)
            if (Mathf.Abs(rigidbody.velocity.x) > 0.1f)
                meshObject.transform.Rotate(Mathf.Sin(Time.time * 10f) / 10f, 0, 0);
        var rot = Quaternion.FromToRotation(transform.up, Vector3.up);
        rigidbody.AddTorque(new Vector3(rot.x, rot.y, rot.z) * 100);
    }

    private void AngleSwingingCharacter() {
        if (currentState == PlayerState.Grappling) {
            meshObject.transform.right = -rigidbody.velocity;
            if (rigidbody.velocity.x <= 0f) {
                meshObject.transform.localScale = new Vector3(1f, 0.64251f, 1f);
            }
            if (rigidbody.velocity.x > 0f) {
                meshObject.transform.localScale = new Vector3(1f, -0.64251f, 1f);
            }
        }
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

    public void MoveToPachinko() {
        for (int i = 0; i < GameManager.Instance.tgm.players.Count; i++) {
            if (gameObject == GameManager.Instance.tgm.players[i]) {
                GameManager.Instance.tgm.targetGroup.m_Targets[i].weight = 0;
                break;
            }
        }
        //if (GameManager.Instance.deadPlayers.Count == 0)
        GameManager.Instance.SpawnThePachinko();
        GameManager.Instance.tgm.players.Remove(gameObject);
        grappleController.EndGrapple();
        throwRocksController.enabled = true;
        throwRocksController.disableThrowing = false;
        grappleController.enabled = false;
        disableJump = true;
        rigidbody.velocity = Vector3.zero;
        transform.SetParent(Camera.main.gameObject.transform);
        transform.position = GameManager.Instance.pachinkoSpawnPoint.transform.position;
        transform.position = GameManager.Instance.pachinkoSpawnPoint.transform.position;
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        GetComponent<Collider>().enabled = false;
        currentState = PlayerState.Pachinker;
        GameManager.Instance.livingPlayers.Remove(gameObject);
        GameManager.Instance.IsLevelEnd();
    }

    public void LoseMoveToPachinko() {
        GameManager.Instance.AddScore(gameObject, false);
        PlayerManager.Instance.deadPlayers.Add(gameObject);
        MoveToPachinko();
    }

    public void WinMoveToPachinko() {
        GameManager.Instance.AddScore(gameObject, true);
        PlayerManager.Instance.wonPlayers.Add(gameObject);
        MoveToPachinko();
    }

    public void MoveToClimbing() {
        Debug.Log("CLIMBING");
        throwRocksController.enabled = false;
        throwRocksController.disableThrowing = true;
        grappleController.enabled = true;
        disableJump = false;
        rigidbody.velocity = Vector3.zero;
        transform.localScale = new Vector3(1f, 1f, 1f);
        rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        GetComponent<Collider>().enabled = true;
        currentState = PlayerState.OnGround;
        if (throwRocksController.rockHolding != null)
            Destroy(throwRocksController.rockHolding);
    }

    private bool OverMaxAirVelocity(Direction direction) {
        if (direction == Direction.Left) {
            if (rigidbody.velocity.x <= -maxAirVelocity) {
                return true;
            }
        } else {
            if (rigidbody.velocity.x >= maxAirVelocity) {
                return true;
            }
        }
        return false;
    }
}
