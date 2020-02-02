using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorLight : MonoBehaviour
{

    public Light light;
    public float intensityMax = 1.5f;

    public float timer;
    public float durationOfErrorLight = 0.5f;

    public void Start()
    {
        timer = durationOfErrorLight;
        light.intensity = Mathf.Lerp(intensityMax, 0, timer / durationOfErrorLight);
    }

    // Start is called before the first frame update
    public void TriggerErrorLight()
    {
        light.intensity = intensityMax;
        timer = 0;
    }

    private void Update()
    {
        timer = Mathf.Min(timer + Time.deltaTime, durationOfErrorLight);
        light.intensity = Mathf.Lerp(intensityMax, 0, timer / durationOfErrorLight);
    }
}
