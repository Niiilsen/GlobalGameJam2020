using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class CinemaLightFlicker : MonoBehaviour
{
    Light myLight;
    float x, y;
    [SerializeField] private Vector2 flickerSpeed;

    [SerializeField] private float minIntensity = 0;
    [SerializeField] private float maxIntensity = 1;


    // Start is called before the first frame update
    void Start()
    {
        myLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        x += Time.deltaTime * flickerSpeed.x;
        y += Time.deltaTime * flickerSpeed.y;
        float randomNumber = Mathf.PerlinNoise(x, y);

        randomNumber = Utils.Remap(randomNumber, 0,1, minIntensity, maxIntensity);
        myLight.intensity = randomNumber;
    }
}
