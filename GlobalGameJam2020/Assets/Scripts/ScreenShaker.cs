using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShaker : MonoBehaviour
{



    [Header("Rotational")]
    [SerializeField]
    private float maxYaw = 40f;
    [SerializeField]
    private float maxPitch = 20f;
    [SerializeField]
    private float maxRoll = 10f;

    [Header("Positional")]
    [SerializeField]
    private float maxOfs = 2f;

    [Header("Settings")]
    [SerializeField]
    private bool useRotationalShake = true;
    [SerializeField]
    private bool usePositionalShake = true;
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float gravity = 1f;
    [SerializeField]
    private int shakeStrength;

    //-----------------------------------//

    private float shake = 0;
    private float trauma = 0;
    private float modifiedTime = 0;
    private int seed = 100;

    private Quaternion finalRotationShake = Quaternion.identity;
    private Vector3 finalPositionalShake = Vector3.zero;

    public Quaternion FinalRotationShake { get { return finalRotationShake; } }
    public Vector3 FinalPositionalShake { get { return finalPositionalShake; } }

    Vector3 initPos;
    Quaternion initRotation;

    // Update is called once per frame

    private void Start()
    {
        initPos = transform.position;
        initRotation = transform.rotation;
    }

    void Update()
    {

        modifiedTime = Time.time * speed;
        if (trauma > 0)
            trauma -= Time.deltaTime * gravity;
        else
            trauma = 0f;

        Shake();


        if (Input.GetKeyDown(KeyCode.T))
        {
            AddTrauma(0.3f);
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            AddTrauma(1f);
        }


    }

    // Stack some trauma to build up a nice shake
    public void AddTrauma(float newTrauma)
    {
        newTrauma *= ((float)shakeStrength/100f);

        trauma += newTrauma;
        trauma = Mathf.Clamp01(trauma);
    }

    //Calculate the shaking
    void Shake()
    {
        shake = Mathf.Pow(trauma, 2);
        finalRotationShake = CalculateRotationalShake();
        finalPositionalShake = CalculatePotitionalShake();

        transform.position = initPos + FinalPositionalShake;
        transform.rotation = initRotation * finalRotationShake;
    }

    //Calculate the rotational shake amount
    Quaternion CalculateRotationalShake()
    {
        if (!useRotationalShake)
            return Quaternion.identity;

        float yaw = maxYaw * shake * GetPerlinNoise(0);
        float pitch = maxPitch * shake * GetPerlinNoise(1);
        float roll = maxRoll * shake * GetPerlinNoise(2);

        return Quaternion.Euler(yaw, pitch, roll);
    }

    //Calculate the positional shake amount
    Vector3 CalculatePotitionalShake()
    {
        if (!usePositionalShake)
            return Vector3.zero;

        float offsetX = maxOfs * shake * GetPerlinNoise(3);
        float offsetY = maxOfs * shake * GetPerlinNoise(4);
        float offsetZ = maxOfs * shake * GetPerlinNoise(5);

        return new Vector3(offsetX, offsetY, offsetZ);
    }

    float GetPerlinNoise(int seedOffset)
    {
        float noise = Mathf.PerlinNoise(seed + seedOffset, modifiedTime);
        noise = (noise - 0.5f) * 2f; //Map it from (0-1) to (-1-1) to get negative values aswell;
        return noise;
    }
}
