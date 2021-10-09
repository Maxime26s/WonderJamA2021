using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    public float minStartAngle, maxStartAngle;
    public Vector2 startForce;
    public Rigidbody rb;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        float angle = Mathf.Deg2Rad * Random.Range(minStartAngle, maxStartAngle);
        rb.AddForce(new Vector3(Mathf.Cos(angle) * startForce.x, Mathf.Sin(angle) * startForce.y), ForceMode.Impulse);
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }
}
