using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject pachinko, boulder, panel;
    public float distance = 5f;
    public int nbBoulders = 5;
    public float boulderSpawnSpeed = 1f;

    private RectTransform panelRectTransform;
    private Vector3[] corners;
    // Start is called before the first frame update
    void Start()
    {
        panelRectTransform = panel.GetComponent<RectTransform>();
        corners = new Vector3[4];
        panelRectTransform.GetWorldCorners(corners);
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.x -= 90;

        for (float j = corners[0].y; j < corners[2].y; j += distance)
        {
            float offset = distance;
            
            if ((Mathf.Abs(corners[0].y) - Mathf.Abs(j)) % 2 == 0)
                offset = distance / 2;
            else
                offset = 0f;

            
            for (float i = corners[0].x; i < corners[2].x; i += distance)
                Instantiate(pachinko, new Vector3(i - offset, j + 2f, corners[0].z), Quaternion.Euler(rotation)); 
        }

        StartCoroutine("SpawnBoulders");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SpawnBoulders()
    {
        for(int i = 0; i < nbBoulders; i++)
            Instantiate(boulder, new Vector3(Random.Range(corners[0].x, corners[2].x), corners[1].y + 5f, corners[0].z), transform.rotation);

        yield return new WaitForSeconds(boulderSpawnSpeed);

        yield return SpawnBoulders();
    }
}
