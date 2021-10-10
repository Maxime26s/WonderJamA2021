using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    public float timer, cd;
    public List<GameObject> rocks;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > cd)
        {
            timer = 0;
            float x = Random.Range(-4.5f, 4.5f);
            GameObject newRock = ThrowRocksController.ProjectBoulder(new Vector3(transform.position.x + x, transform.position.y, transform.position.z), rocks, offset);
            newRock.transform.localScale *= 1.5f;
        }
    }
}
