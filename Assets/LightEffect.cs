using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEffect : MonoBehaviour
{
    public Light pointLight;
    float time = 0, half = 0.5f, duration = 2, intensity = 10;
    // Start is called before the first frame update
    void Start()
    {
        pointLight.intensity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time < half)
            pointLight.intensity += intensity / half * Time.deltaTime;
        else if (time < duration)
            pointLight.intensity += -intensity / (duration - half) * Time.deltaTime;
    }
}
