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
            if (topBar == null || rockHolding == null && Camera.main != null)
            {
                HoldRock(0);
                topBar = Camera.main.transform.GetComponent<ObjectHolder>().GOs[0];
            }

            Destroy(rockHolding);
            disableThrowing = true;

            GameObject newRock = ProjectBoulder(rockHolding.transform.position, rocks, offset, nextRock);

            newRock.transform.localScale *= 1.5f;

            StartCoroutine(CoolDown());
            nextRock = Random.Range(0, rocks.Count);

            Physics.IgnoreCollision(newRock.GetComponent<Collider>(), HoldRock(nextRock).GetComponent<Collider>());
            Physics.IgnoreCollision(newRock.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
            Physics.IgnoreCollision(newRock.GetComponent<Collider>(), topBar.GetComponent<Collider>());
        }
    }

    private GameObject HoldRock(int rockIndex)
    {
        rockHolding = Instantiate(rocks[rockIndex]);
        rockHolding.GetComponent<Boulder>().enabled = false;
        rockHolding.GetComponent<Boulder>().isHeld = true;
        rockHolding.transform.position = transform.position + new Vector3(0, 0.25f, 0);
        rockHolding.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
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

    public static GameObject ProjectBoulder(Vector3 rockPosition, List<GameObject> rocks, Vector3 offset, int nextRock = -1)
    {
        Vector3 direction = rockPosition - Camera.main.transform.position;
        direction.Normalize();
        float multiplier = (0 - Camera.main.transform.position.z) / direction.z;
        if (nextRock == -1)
            nextRock = Random.Range(0, rocks.Count);
        GameObject newRock = Instantiate(rocks[nextRock]);
        newRock.transform.position = Camera.main.transform.position + direction * multiplier;
        newRock.transform.position = new Vector3(newRock.transform.position.x, rockPosition.y + offset.y, 0);
        return newRock;
    }
}
