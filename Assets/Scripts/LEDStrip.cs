using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum direction
{
    x,
    y,
    z
}

public class LEDStrip : MonoBehaviour
{
    public Color color;
    public float lightDistance = 5;
    public float lightSize = 5;
    public float intensity = 5;
    public direction direction;
    public GameObject light;
    public bool randomColor;
    public float flashingSpeed;
    public float delay = 0f;

    private float distance;
    private Vector3 rotation;
    private GameObject newObject;
    private LEDLight lightScript;
    public List<GameObject> lights;

    // Start is called before the first frame update
    void Start()
    {
        switch(direction)
        {
            case direction.x:
                distance = gameObject.GetComponent<Renderer>().bounds.size.x;
                break;
            case direction.y:
                distance = gameObject.GetComponent<Renderer>().bounds.size.y;
                break;
            case direction.z:
                distance = gameObject.GetComponent<Renderer>().bounds.size.z;
                break;
        }

        for (float i = 0; i < distance; i += lightDistance + lightSize)
        {
            switch (direction)
            {
                case direction.x:
                    rotation = transform.rotation.eulerAngles;
                    rotation.z += 90;
                    rotation.y += 90;
                    newObject = Instantiate(light, new Vector3(transform.position.x + i, transform.position.y + gameObject.GetComponent<Renderer>().bounds.size.y / 2, transform.position.z), Quaternion.Euler(rotation));
                    newObject.transform.position = new Vector3(newObject.transform.position.x, newObject.transform.position.y - newObject.GetComponent<Renderer>().bounds.size.y / 2, newObject.transform.position.z);
                    newObject.transform.localScale = new Vector3(lightSize / 5, 1, lightSize / 5);

                    break;
                case direction.y:
                    rotation = transform.rotation.eulerAngles;
                    rotation.x += 90;
                    newObject = Instantiate(light, new Vector3(transform.position.x + gameObject.GetComponent<Renderer>().bounds.size.x / 2, transform.position.y + i, transform.position.z), Quaternion.Euler(rotation));
                    newObject.transform.position = new Vector3(newObject.transform.position.x, newObject.transform.position.y - newObject.GetComponent<Renderer>().bounds.size.y / 2, newObject.transform.position.z);
                    newObject.transform.localScale = new Vector3(lightSize / 5, 1, lightSize / 5);
                    break;
                case direction.z:
                    rotation = transform.rotation.eulerAngles;
                    rotation.x += 90;
                    rotation.z += 90;
                    newObject = Instantiate(light, new Vector3(transform.position.x, transform.position.y + gameObject.GetComponent<Renderer>().bounds.size.y / 2, transform.position.z + i), Quaternion.Euler(rotation));
                    break;
            }

            lights.Add(newObject);
        }

        StartCoroutine("StartLights");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator StartLights()
    {
        foreach(GameObject light in lights)
        {
            lightScript = light.GetComponent<LEDLight>();
            lightScript.color = color;
            lightScript.intensity = intensity;
            lightScript.randomColor = randomColor;
            lightScript.flashingSpeed = flashingSpeed;

            yield return new WaitForSeconds(delay);
        }
    }
}
