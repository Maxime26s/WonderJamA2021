using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    public float duration, angle, power;
    public float restAngle;
    public bool flipping = false;
    public GameObject endPoint;

    public GameObject flipParticles = null;

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
            transform.parent.transform.eulerAngles = new Vector3(transform.parent.transform.eulerAngles.x, transform.parent.transform.eulerAngles.y, angle);
            while (time > 0)
            {
                time -= Time.deltaTime;
                transform.parent.transform.eulerAngles -= new Vector3(0, 0, angleBySecond) * Time.deltaTime;
                yield return null;
            }
            transform.parent.transform.eulerAngles = new Vector3(transform.parent.transform.eulerAngles.x, transform.parent.transform.eulerAngles.y, restAngle);
            flipping = false;
        }
        if (!flipping)
        {
            if (collision.gameObject.TryGetComponent(out Rigidbody rb))
            {
                if ((collision.transform.position - collision.contacts[0].point).y > 0)
                {
                    StartCoroutine(Flip(duration, angle, restAngle));
                    Vector3 calculatedForce = CalculateForce(collision.transform.position, collision.contacts[0].point);
                    calculatedForce = new Vector3(calculatedForce.x, calculatedForce.y, 0f);
                    GameObject particles = Instantiate(flipParticles, collision.contacts[0].point, Quaternion.identity);
                    particles.transform.forward = calculatedForce;
                    rb.AddForce(calculatedForce, ForceMode.VelocityChange);
                }
            }
        }
    }

    public Vector3 CalculateForce(Vector3 objectPosition, Vector3 contactPoint)
    {
        float proportionalForce = Mathf.Clamp((contactPoint - transform.parent.transform.position).magnitude / (endPoint.transform.position - transform.parent.transform.position).magnitude, 0.5f, 1);
        return (objectPosition - contactPoint).normalized * power * proportionalForce;
    }
}
