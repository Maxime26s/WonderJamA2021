using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testshit : MonoBehaviour
{
    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
            Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
    }
}
