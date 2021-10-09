using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowRocksController : MonoBehaviour
{
    public List<GameObject> rocks;
    public int nextRock = 0;
    public float rocksCoolDown;
    public bool disableThrowing = false;
    public GameObject rockHolding;

    private void Start()
    {
        HoldRock(0);
    }



    public void OnGrapple()
    {
        if (!disableThrowing)
        {
            Destroy(rockHolding);
            disableThrowing = true;
            GameObject newRock = Instantiate(rocks[nextRock]);
            newRock.transform.position = transform.position + new Vector3(0, -1.5f, 0);
            StartCoroutine(CoolDown());
            nextRock = Random.Range(0, 5);
            HoldRock(nextRock);
        }
    }

    private void HoldRock(int rockIndex)
    {
        rockHolding = Instantiate(rocks[rockIndex]);
        rockHolding.transform.position = transform.position + new Vector3(0, 1f, 0);
        rockHolding.transform.SetParent(transform);
        rockHolding.GetComponent<Collider>().enabled = false;
        rockHolding.GetComponent<Rigidbody>().isKinematic = true;
    }

    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(rocksCoolDown);
        disableThrowing = false;
    }
}
