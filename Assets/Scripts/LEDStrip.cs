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
    public float intensity = 255;
    public direction direction;
    public GameObject light;

    private float distance;
    private Vector3 rotation;
    
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

        for(float i = distance; i > 0; i -= lightDistance + lightSize)
        {
            switch (direction)
            {
                case direction.x:
                    rotation = transform.rotation.eulerAngles;
                    rotation.y += 90;
                    Instantiate(light, new Vector3(transform.position.x + i, transform.position.y + gameObject.GetComponent<Renderer>().bounds.size.y / 2, transform.position.z), Quaternion.Euler(rotation));
                    break;
                case direction.y:
                    rotation = transform.rotation.eulerAngles;
                    rotation.x += 90;
                    GameObject newObject = Instantiate(light, new Vector3(transform.position.x + gameObject.GetComponent<Renderer>().bounds.size.x / 2, transform.position.y + i, transform.position.z), Quaternion.Euler(rotation));
                    newObject.transform.position = new Vector3(newObject.transform.position.x, newObject.transform.position.y - newObject.GetComponent<Renderer>().bounds.size.y / 2, newObject.transform.position.z);
                    newObject.transform.localScale = new Vector3(lightSize / 5, 1, lightSize / 5);
                    Material material = newObject.GetComponent<MeshRenderer>().material;
                    material.color = color;
                    material.SetColor("_EmissionColor", color);
                    break;
                case direction.z:
                    rotation = transform.rotation.eulerAngles;
                    rotation.y += 90;
                    Instantiate(light, new Vector3(transform.position.x, transform.position.y + gameObject.GetComponent<Renderer>().bounds.size.y / 2, transform.position.z + i), Quaternion.Euler(rotation));
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
