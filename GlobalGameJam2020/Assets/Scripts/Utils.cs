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

    // Kinematic Equation from Sebastian Lague @ https://www.youtube.com/watch?v=IvT8hjy6q4o&index=3&list=PLFt_AvWsXl0eMryeweK7gc9T04lJCIg_W
    public static ThrowData CalculateThrowData(Vector3 origin, Vector3 target, float height, float gravity) {
        float displacementY = target.y - origin.y;

        Vector3 displacementXZ = new Vector3(target.x - origin.x, 0, target.z - origin.z);

        float time = Mathf.Sqrt(-2 * height / gravity) + Mathf.Sqrt(2 * (displacementY - height) / gravity);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);

        Vector3 velocityXZ = displacementXZ / time;

        return new ThrowData(velocityXZ + velocityY, time);
    }

    public struct ThrowData {
        public readonly Vector3 velocity;
        public readonly float time;

        public ThrowData(Vector3 velocity, float time) {
            this.velocity = velocity;
            this.time = time;
        }
    }

    public static float Remap(float value, float newFrom, float newTo, float oldFrom, float oldTo) {
        return (value - newFrom) / (newTo - newFrom) * (oldTo - oldFrom) + oldFrom;
    }
}
