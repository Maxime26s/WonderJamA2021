using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    public float minStartAngle, maxStartAngle;
    public Vector2 startForce;
    public Rigidbody rb;
    public EffectController effectController;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        float angle = Mathf.Deg2Rad * Random.Range(minStartAngle, maxStartAngle);
        rb.AddForce(new Vector3(Mathf.Cos(angle) * startForce.x, Mathf.Sin(angle) * startForce.y), ForceMode.Impulse);
        effectController = Camera.main.gameObject.GetComponent<EffectController>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude >= effectController.shakeThreshold)
            effectController.StartShake(collision.relativeVelocity.magnitude);
    }
}
