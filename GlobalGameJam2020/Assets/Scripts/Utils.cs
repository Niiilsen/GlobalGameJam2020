using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {
    public static Vector3 GetPointOnUnitSphereCap(Quaternion targetDirection, float angle) {
        var angleInRad = Random.Range(0.0f, angle) * Mathf.Deg2Rad;
        var PointOnCircle = (Random.insideUnitCircle.normalized) * Mathf.Sin(angleInRad);
        var V = new Vector3(PointOnCircle.x, PointOnCircle.y, Mathf.Cos(angleInRad));
        return targetDirection * V;
    }

    public static Vector3 GetPointOnUnitSphereCap(Vector3 targetDirection, float angle) {
        return GetPointOnUnitSphereCap(Quaternion.LookRotation(targetDirection), angle);
    }
}
