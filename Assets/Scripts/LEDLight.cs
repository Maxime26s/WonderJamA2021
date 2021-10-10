using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LEDLight : MonoBehaviour
{
    public Color color;
    public float intensity = 5;
    public bool randomColor = false;
    public float flashingSpeed = 0f;
    public bool flashOnce = false;

    private bool flashing = false;
    private Material material;

    // Start is called before the first frame update
    void Start()
    {
        material = gameObject.GetComponent<MeshRenderer>().material;
 
        material.SetColor("_Color", color);
        material.SetColor("_EmissionColor", color * 0);

        if (flashingSpeed > 0)
            StartCoroutine("Flash");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (flashingSpeed > 0 && !flashing)
            StartCoroutine("Flash");
    }

    public IEnumerator Flash()
    {
        flashing = true;
        if (randomColor)
            color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);

        if(!flashOnce)
        {
            material.SetColor("_EmissionColor", color * intensity);
            yield return new WaitForSeconds(flashingSpeed);
            material.SetColor("_EmissionColor", color * 0);
        }
        else
        {
            material.SetColor("_EmissionColor", color * intensity);
            yield return new WaitForSeconds(flashingSpeed);
            material.SetColor("_EmissionColor", color * 0);
        }

        yield return new WaitForSeconds(flashingSpeed);

        if (!flashOnce)
            yield return Flash();
    }
}
