using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public float magnitude, duration, shakeThreshold, maxShake;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartShake(float velocity)
    {
        IEnumerator Shake(float duration, float magnitude)
        {
            Vector3 originalPos = transform.localPosition;
            float elapsed = 0.0f;
            while (elapsed < duration)
            {
                Vector2 range = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * magnitude;
                transform.localPosition = new Vector3(originalPos.x + range.x, originalPos.y + range.y, originalPos.z);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = originalPos;
        }
        StartCoroutine(Shake(duration * (velocity - shakeThreshold) / (maxShake - shakeThreshold), magnitude * (velocity - shakeThreshold) / (maxShake - shakeThreshold)));
    }

}
