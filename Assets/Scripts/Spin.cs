using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        transform.eulerAngles = new Vector3(30 * Mathf.Cos(timer), 45 * Mathf.Sin(timer) - 90, 20 * Mathf.Sin(timer));
    }
}
