using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorLever : MonoBehaviour {
    public Conveyor conveyor;

    public void Trigger(Team t) {
        if(t != conveyor.team) {
            return;
        }

        conveyor.Toggle();
    }
}
