using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField] Material belt;
    [SerializeField] private float beltSpeed = 0.25f;
    [SerializeField] private float lerpSpeed = 1f;

    private float targetSpeed = 0;
    private float currentSpeed = 0;
    private float currentOffset = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBeltVisuals();        
    }

    public void ToggleBelt(bool value)
    {
        targetSpeed = value ? beltSpeed : 0;
    }

    void UpdateBeltVisuals()
    {
        targetSpeed = beltSpeed / 100;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, lerpSpeed * Time.deltaTime);

        currentOffset += currentSpeed;
        belt.SetTextureOffset("_MainTex", new Vector2(0, currentOffset));
    }
}


