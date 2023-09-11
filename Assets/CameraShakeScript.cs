using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShakeScript : MonoBehaviour
{
    private CinemachineVirtualCamera cvc;
    public float shakeIntensity;
    public float shakeTime;

    private float timer;
    private CinemachineBasicMultiChannelPerlin cbmcp;

    private void Awake()
    {
        cvc = GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        StopShake();
    }

    public void ShakeCamera()
    {
        CinemachineBasicMultiChannelPerlin cbmcp = cvc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbmcp.m_AmplitudeGain = shakeIntensity;

        timer = shakeTime;
    }

    void StopShake()
    {
        CinemachineBasicMultiChannelPerlin cbmcp = cvc?.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbmcp.m_AmplitudeGain = 0f;
        timer = 0;
    }

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                StopShake();
            }
        }
    }
}
