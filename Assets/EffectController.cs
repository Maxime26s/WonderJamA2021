using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EffectController : MonoBehaviour
{
    public static EffectController Instance { get; private set; }

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer, startingIntensity, shakeTimerTotal;
    public float magnitude, duration, shakeThreshold, maxShake;

    private void Awake()
    {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    // Start is called before the first frame update
    public void ShakeCamera(float intensity)
    {
        if (cinemachineVirtualCamera == null)
            return;
        float calculatedDuration = duration * Mathf.Clamp((intensity - shakeThreshold) / (maxShake - shakeThreshold), 0, 1);
        float calculatedIntensity = magnitude * Mathf.Clamp((intensity - shakeThreshold) / (maxShake - shakeThreshold), 0, 1);

        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = calculatedIntensity;

        startingIntensity = calculatedIntensity;
        shakeTimerTotal = calculatedDuration;
        shakeTimer = calculatedDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
                Mathf.Lerp(startingIntensity, 0f, 1 - shakeTimer / shakeTimerTotal);
            }
        }

    }
}
