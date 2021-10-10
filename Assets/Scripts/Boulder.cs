using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    public float minStartAngle, maxStartAngle;
    public Vector2 startForce;
    public Rigidbody rb;
    public GameObject deathParticle;
    public bool isHeld = false;
    public List<AudioClip> boulderSounds = null;
    public AudioSource boulderAudioSource = null;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Destroy(gameObject, 12.5f);
        float angle = Mathf.Deg2Rad * Random.Range(minStartAngle, maxStartAngle);
        rb.AddForce(new Vector3(Mathf.Cos(angle) * startForce.x, Mathf.Sin(angle) * startForce.y), ForceMode.Impulse);
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (boulderAudioSource != null && boulderSounds != null && boulderSounds.Count > 0) {
            boulderAudioSource.volume = Mathf.Clamp(collision.relativeVelocity.magnitude / 50f, 0f, 1f);
            boulderAudioSource.PlayOneShot(boulderSounds[Random.Range(0, boulderSounds.Count)]);
        }
        if (collision != null && EffectController.Instance != null && collision.relativeVelocity.magnitude >= EffectController.Instance.shakeThreshold)
        {
            EffectController.Instance.ShakeCamera(collision.relativeVelocity.magnitude);
            if (collision.gameObject.TryGetComponent(out GrappleController grapple))
                grapple.EndGrapple();
        }
    }

    protected virtual void OnDestroy()
    {
        CancelInvoke();
        if(!isHeld && deathParticle != null)
            Instantiate(deathParticle, transform.position, Quaternion.identity);
    }
}
