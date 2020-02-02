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

    FMODUnity.StudioEventEmitter audioSource;
    [SerializeField, FMODUnity.EventRef] string conveyorBeltEvent;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<FMODUnity.StudioEventEmitter>();
        audioSource.Event = conveyorBeltEvent;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBeltVisuals();        
    }

    public void ToggleBelt(bool value)
    {
        if (value)
        {
            currentSpeed = beltSpeed;
            audioSource.Play();
        }
        else
        {
            currentSpeed = 0;
            audioSource.Stop();
        }
    }

    void UpdateBeltVisuals()
    {
        //targetSpeed = beltSpeed / 100;
        //currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, lerpSpeed * Time.deltaTime);

        currentOffset += currentSpeed * Time.deltaTime;
        belt.SetFloat("_TrackSpeed", currentOffset);
    }
}


