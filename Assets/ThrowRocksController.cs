using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowRocksController : MonoBehaviour
{
    public List<GameObject> rocks;
    public int nextRock = 0;
    public float rocksCoolDown;
    public bool disableThrowing = false;
    public GameObject rockHolding, topBar;
    public Vector3 offset;

    private void Start()
    {
        topBar = Camera.main.transform.GetComponentInChildren<PlaneCalculator>().gameObject;
        HoldRock(0);
    }

    public void OnGrapple()
    {
        if (!disableThrowing)
        {
            Destroy(rockHolding);

            Vector3 direction = rockHolding.transform.position - Camera.main.transform.position;
            direction.Normalize();
            float multiplier = (0 - Camera.main.transform.position.z) / direction.z;

            disableThrowing = true;
            GameObject newRock = Instantiate(rocks[nextRock]);
            newRock.transform.position = transform.position + offset;
            newRock.transform.position = new Vector3(newRock.transform.position.x, newRock.transform.position.y, 0);
            newRock.transform.position = Camera.main.transform.position + direction * multiplier;

            StartCoroutine(CoolDown());
            nextRock = Random.Range(0, 5);

            Physics.IgnoreCollision(newRock.GetComponent<Collider>(), HoldRock(nextRock).GetComponent<Collider>());
            Physics.IgnoreCollision(newRock.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
            Physics.IgnoreCollision(newRock.GetComponent<Collider>(), topBar.GetComponent<Collider>());
        }
    }

    private GameObject HoldRock(int rockIndex)
    {
        rockHolding = Instantiate(rocks[rockIndex]);
        rockHolding.GetComponent<Boulder>().enabled = false;
        rockHolding.transform.position = transform.position + new Vector3(0, 1f, 0);
        rockHolding.transform.SetParent(transform);
        rockHolding.GetComponent<Collider>().enabled = false;
        rockHolding.GetComponent<Rigidbody>().isKinematic = true;
        return rockHolding;
    }

    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(rocksCoolDown);
        disableThrowing = false;
    }
}
