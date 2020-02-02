using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorLever : MonoBehaviour {
    public Conveyor conveyor;
    public Animator animator;

    [SerializeField, FMODUnity.EventRef] string toggleSnd;

    public void Trigger(Team t) {
        if(t != conveyor.team) {
            return;
        }

        conveyor.Toggle();

        animator.SetBool("On", conveyor.Moving);
        FMODUnity.RuntimeManager.PlayOneShot(toggleSnd, transform.position);
    }
}
