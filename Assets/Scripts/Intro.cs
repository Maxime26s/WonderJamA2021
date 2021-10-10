using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    public GameObject faceCamera;
    public GameObject fallCamera;
    public GameObject canvas;
    public bool walking;

    private bool firstTime = true;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("SpinLeft");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SpinLeft()
    {
        yield return new WaitForSeconds(2f);

        for(float i = 0; i < 90f; i += 1f)
        {
            transform.Rotate(0f, -1f, 0.0f, Space.Self);
            yield return new WaitForSeconds(0.01f);
        }

        yield return StartWalkingToWell();
    }

    public IEnumerator WalkingAnimation(bool left)
    {
        if(left)
        {
            if(firstTime)
            {
                firstTime = false;
                for (float i = 0; i < 20f; i += 1f)
                {
                    transform.Rotate(1f, 0f, 0f, Space.Self);
                    yield return new WaitForSeconds(0.01f);
                }
            }
            else
            {
                for (float i = 0; i < 40f; i += 1f)
                {
                    transform.Rotate(1f, 0f, 0f, Space.Self);
                    yield return new WaitForSeconds(0.01f);
                }
            }

            left = false;
        }
        else
        {
            for (float i = 0; i < 40f; i += 1f)
            {
                transform.Rotate(-1f, 0f, 0f, Space.Self);
                yield return new WaitForSeconds(0.01f);
            }

            left = true;
        }

        if(walking)
            yield return WalkingAnimation(left);
    }

    public IEnumerator StartWalkingToWell()
    {
        StartCoroutine(WalkingAnimation(true));
        walking = true;
        for (float i = 0; i < 300f; i += 1f)
        {
            transform.position = new Vector3(transform.position.x + 0.03f, transform.position.y, transform.position.z);
            yield return new WaitForSeconds(0.01f);
        }

        yield return Fall();
    }

    public IEnumerator Fall()
    {
        walking = false;
        transform.Rotate(0f, 0f, 0f, Space.Self);
        yield return new WaitForSeconds(1f);

        faceCamera.transform.Rotate(0f, 0, -90f, Space.Self);

        for (float i = 0; i < 90f; i += 1f)
        {
            transform.Rotate(0f, 0, +1f, Space.Self);
            yield return new WaitForSeconds(0.01f);
        }

        yield return FallThrough();
    }

    public IEnumerator FallThrough()
    {
        for (float i = 0; i < 390f; i += 1f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.03f, transform.position.z);
            yield return new WaitForSeconds(0.01f);
        }

        yield return FallInWell();
    }

    public IEnumerator FallInWell()
    {
        faceCamera.GetComponent<Camera>().enabled = false;
        fallCamera.GetComponent<Camera>().enabled = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = true;

        yield return null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "ExplosionIntro")
        {
            faceCamera.GetComponent<Camera>().enabled = true;
            fallCamera.GetComponent<Camera>().enabled = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            //gameObject.transform.rotation = new Vector3(0, 0, 0);

            GameObject player1 = Instantiate(gameObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            player1.GetComponent<MeshRenderer>().material.color = new Color32(230, 126, 23, 255);
            player1.GetComponent<Intro>().enabled = false;
            GameObject player2 = Instantiate(gameObject, new Vector3(transform.position.x, transform.position.y, transform.position.z + 2f), Quaternion.identity);
            player2.GetComponent<MeshRenderer>().material.color = new Color32(41, 204, 41, 255);
            player2.GetComponent<Intro>().enabled = false;
            GameObject player3 = Instantiate(gameObject, new Vector3(transform.position.x, transform.position.y, transform.position.z + 4f), Quaternion.identity);
            player3.GetComponent<MeshRenderer>().material.color = new Color32(41, 204, 204, 255);
            player3.GetComponent<Intro>().enabled = false;
            GameObject player4 = Instantiate(gameObject, new Vector3(transform.position.x, transform.position.y, transform.position.z + 6f), Quaternion.identity);
            player4.GetComponent<MeshRenderer>().material.color = new Color32(41, 204, 255, 255);
            player4.GetComponent<Intro>().enabled = false;

            var cameras = GameObject.FindGameObjectsWithTag("MainCamera");
            foreach(GameObject camera in cameras)
                camera.transform.Rotate(0f, 0f, 90f, Space.Self);

            StartCoroutine("Finish");
        }
    }

    public IEnumerator Finish()
    {
        yield return new WaitForSeconds(2f);
        canvas.SetActive(true);
        yield return new WaitForSeconds(2f);
        OnStartAction();
    }

    public void OnStartAction()
    {
        GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>().LoadMenu();
    }
}
