using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationCurveVariable", menuName = "Scriptable Object/Special/Animation/Curve", order = 1)]
public class AnimationCurveVariable : ScriptableObject {

    [SerializeField]
    private AnimationCurve variable;
    public AnimationCurve Variable
    {
        get
        {
            return variable;
        }
        set
        {
            variable = value;
        }
    }
}
