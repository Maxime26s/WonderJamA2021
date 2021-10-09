using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DividedBoulder : Boulder
{
    public int life;
    public float cdTime, divisionPower;
    public bool cd = false;
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

        if (!cd)
        {
            if (--life <= 0)
                Destroy(gameObject);
            else
            {
                GetComponent<SphereCollider>().enabled = false;
                GameObject go1 = Instantiate(gameObject);
                go1.transform.localScale *= 0.8f;
                go1.GetComponent<SphereCollider>().enabled = true;
                go1.GetComponent<DividedBoulder>().StartCD();
                GameObject go2 = Instantiate(go1);
                Physics.IgnoreCollision(go1.GetComponent<Collider>(), go2.GetComponent<Collider>());
                go2.GetComponent<DividedBoulder>().StartCD();

                Vector3 point = collision.contacts[0].point;
                Vector3 normal = (point - gameObject.transform.position).normalized;
                Vector3 velocity = Vector3.Dot(rb.velocity, normal) * normal;
                Rigidbody rb1 = go1.GetComponent<Rigidbody>();
                Rigidbody rb2 = go2.GetComponent<Rigidbody>();

                rb1.velocity = new Vector3(Mathf.Sqrt(velocity.x * velocity.x), velocity.y);
                rb2.velocity = new Vector3(-Mathf.Sqrt(velocity.x * velocity.x), velocity.y);

                rb1.AddForce(new Vector3(1f, 1, 0).normalized * divisionPower);
                rb2.AddForce(new Vector3(-1f, 1, 0).normalized * divisionPower);

                if (collision.gameObject.TryGetComponent(out Pachinko pachinko))
                {
                    rb1.AddForce((collision.transform.position - point) * pachinko.power, ForceMode.VelocityChange);
                    rb2.AddForce((collision.transform.position - point) * pachinko.power, ForceMode.VelocityChange);
                }
                if (collision.gameObject.TryGetComponent(out Flipper flipper))
                {
                    rb1.AddForce(flipper.CalculateForce(go1.transform.position, point), ForceMode.VelocityChange);
                    rb2.AddForce(flipper.CalculateForce(go2.transform.position, point), ForceMode.VelocityChange);
                }

                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!cd)
        {
            transform.localScale *= 0.9f;
            if (transform.localScale.x < 0.1f)
                Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        CancelInvoke();
        //Summon particles
    }

    public void StartCD()
    {
        IEnumerator CD()
        {
            cd = true;
            yield return new WaitForSeconds(cdTime);
            cd = false;
        }
        StartCoroutine(CD());
    }
}
