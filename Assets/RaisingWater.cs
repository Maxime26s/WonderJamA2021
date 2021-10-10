using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaisingWater : MonoBehaviour
{
    public float timeBeforeRise, riseSpeed, timeRunning;
    public bool rising;
    public GameObject fakeWater;

    // Start is called before the first frame update
    void Start()
    {
        timeRunning = 0;
        fakeWater.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", GetComponent<MeshRenderer>().material.GetColor("Color_7D9A58EC"));
    }

    // Update is called once per frame
    void Update()
    {
        if (!rising)
        {
            timeRunning += Time.deltaTime;
            if (timeRunning > timeBeforeRise)
                rising = true;
        }
        else if (rising)
        {
            fakeWater.transform.localScale += new Vector3(0, Time.deltaTime * riseSpeed, 0);
            fakeWater.transform.position -= new Vector3(0, Time.deltaTime * riseSpeed * 0.5f, 0);
            transform.parent.transform.position += new Vector3(0, Time.deltaTime * riseSpeed, 0);
        }

    }
}
