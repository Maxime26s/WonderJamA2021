using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBoulder : Boulder
{
    public AudioSource boomAudioSource = null;
    public AudioClip boomSound = null;
    public int life;
    public float ttl, radius, power;
    int maxLife;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        Destroy(gameObject, ttl);
        maxLife = life;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (--life <= 0)
            Destroy(gameObject);
        else
        {
            Color color = new Color(1, (float)(life - 1) / maxLife, 0, 1);
            GetComponent<Renderer>().material.color = color;
        }
    }

    protected override void OnDestroy()
    {
        if (boomAudioSource != null && boomSound != null)
            boomAudioSource.PlayOneShot(boomSound);
        base.OnDestroy();
        if (!isHeld)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider hit in colliders)
            {
                if (hit.TryGetComponent(out Rigidbody rb))
                    rb.AddExplosionForce(power, transform.position, radius);
            }
            EffectController.Instance.ShakeCamera(50);
        }
    }
}
