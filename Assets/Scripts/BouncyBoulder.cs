using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBoulder : Boulder
{
    public float bounceMultiplier;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        Vector3 point = collision.contacts[0].point;
        Vector3 normal = (point - gameObject.transform.position).normalized;
        Vector3 velocity = Vector3.Dot(rb.velocity, normal) * normal;
        rb.AddForce(velocity * bounceMultiplier, ForceMode.VelocityChange);
        if (collision.gameObject.TryGetComponent(out Rigidbody otherRb) && !collision.gameObject.TryGetComponent(out BouncyBoulder bouncyBoulder))
        {
            velocity = Vector3.Dot(rb.velocity, -normal) * -normal;
            otherRb.AddForce(velocity * bounceMultiplier, ForceMode.VelocityChange);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
