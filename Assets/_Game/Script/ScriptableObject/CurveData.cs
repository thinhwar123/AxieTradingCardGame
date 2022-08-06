using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CurveData", menuName = "ScriptableObjects/CurveData", order = 1)]
public class CurveData : ScriptableObject
{
    public AnimationCurve Curve;

    public float Evaluate(float time)
    {
        return Curve.Evaluate(time);
    }
}


