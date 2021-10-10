using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitLight : MonoBehaviour
{
    public List<GameObject> hitLights;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach(GameObject hitLight in hitLights)
        {
            hitLight.GetComponent<LEDLight>().HitFlash();
        }
    }
}
