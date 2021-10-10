using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pachinko : MonoBehaviour
{
    public float power;
    public GameObject flipParticles = null;

    public AudioSource boingAudioSource = null;
    public List<AudioClip> boingAudioClips = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        boingAudioSource.PlayOneShot(boingAudioClips[Random.Range(0,boingAudioClips.Count)]);

        if (collision.gameObject.TryGetComponent(out Rigidbody rb))
        {
            Vector3 point = collision.contacts[0].point;
            Vector3 normal = (point - gameObject.transform.position).normalized;
            Vector3 velocity = Vector3.Dot(rb.velocity, normal) * normal;

            rb.velocity = velocity;
            GameObject particles = Instantiate(flipParticles, collision.contacts[0].point, Quaternion.identity);
            particles.transform.forward = (collision.transform.position - point);
            rb.AddForce((collision.transform.position - point) * power, ForceMode.VelocityChange);
        }
    }
}
