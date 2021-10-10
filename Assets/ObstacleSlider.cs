using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSlider : MonoBehaviour
{
    public GameObject point1, point2, obstacle;
    public Vector3 direction;
    public float speed, stopDuration, distance, stopTimer;
    public bool toggle;
    // Start is called before the first frame update
    void Start()
    {
        direction = (point1.transform.position - point2.transform.position);
        direction = new Vector3(direction.x, direction.y).normalized;
        obstacle.transform.position = point1.transform.position;
        toggle = false;
        distance = ((Vector2)(point1.transform.position - transform.parent.transform.position)).magnitude;
        stopTimer = Mathf.Infinity;
    }

    // Update is called once per frame
    void Update()
    {
        if (stopTimer < stopDuration)
        {
            stopTimer += Time.deltaTime;
        }
        else if (toggle)
        {
            obstacle.transform.position += direction * speed * Time.deltaTime;
            Vector2 distance2d = (obstacle.transform.position - transform.parent.transform.position);
            if (distance < distance2d.magnitude)
            {
                obstacle.transform.position = new Vector3(point1.transform.position.x, point1.transform.position.y, obstacle.transform.position.z);
                toggle = false;
                stopTimer = 0;
            }
        }
        else if (!toggle)
        {
            obstacle.transform.position -= direction * speed * Time.deltaTime;
            Vector2 distance2d = (obstacle.transform.position - transform.parent.transform.position);
            if (distance < distance2d.magnitude)
            {
                obstacle.transform.position = new Vector3(point2.transform.position.x, point2.transform.position.y, obstacle.transform.position.z);
                toggle = true;
                stopTimer = 0;
            }
        }
    }
}
