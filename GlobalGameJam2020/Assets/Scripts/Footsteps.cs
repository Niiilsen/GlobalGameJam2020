using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField, FMODUnity.EventRef] private string footStepEvnt;
    [SerializeField] PlayerCharacterController playerController;
    [SerializeField] private AnimationCurveVariable animCurve;

    float timeOfLast = 0;
    float delayBetweenSteps = 0.1f;

    // Update is called once per frame
    void Update()
    {
        float delayBetweenSteps = animCurve.Variable.Evaluate(playerController.rb.velocity.sqrMagnitude);
        if (playerController.rb.velocity.sqrMagnitude > 5)
        {
            if(Time.time >= timeOfLast + delayBetweenSteps)
            {
                
                FMODUnity.RuntimeManager.PlayOneShot(footStepEvnt, transform.position);
                timeOfLast = Time.time;
            }
        }
        else
        {
            timeOfLast = Time.time;
        }
    }
}
