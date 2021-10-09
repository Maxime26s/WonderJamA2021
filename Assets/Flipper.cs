using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    public float duration, angle, power;
    public float restAngle;
    public bool flipping = false;
    public GameObject endPoint;

    // Start is called before the first frame update
    void Start()
    {
        restAngle = transform.parent.transform.localEulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        IEnumerator Flip(float duration, float angle, float restAngle)
        {
            flipping = true;
            float time = 0;
            float angleBySecond = (angle - restAngle) / duration;
            while (time < duration)
            {
                time += Time.deltaTime;
                transform.parent.transform.eulerAngles += new Vector3(0, 0, angleBySecond) * Time.deltaTime;
                yield return null;
            }
            transform.parent.transform.eulerAngles = new Vector3(0, 0, angle);
            while (time > 0)
            {
                time -= Time.deltaTime;
                transform.parent.transform.eulerAngles -= new Vector3(0, 0, angleBySecond) * Time.deltaTime;
                yield return null;
            }
            transform.parent.transform.eulerAngles = new Vector3(0, 0, restAngle);
            flipping = false;
        }
        if (!flipping)
        {
            StartCoroutine(Flip(duration, angle, restAngle));
            if (collision.gameObject.TryGetComponent(out Rigidbody rb))
            {
                rb.AddForce(CalculateForce(collision.transform.position, collision.contacts[0].point), ForceMode.VelocityChange);
            }
        }
    }

    public Vector3 CalculateForce(Vector3 objectPosition, Vector3 contactPoint)
    {
        float proportionalForce = (contactPoint - transform.parent.transform.position).magnitude / (endPoint.transform.position - transform.parent.transform.position).magnitude;
        return (objectPosition - contactPoint) * power * proportionalForce;
    }
}
