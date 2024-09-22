using UnityEngine;

public class BezierRoute : MonoBehaviour
{
    [Header("Control points for a cubic bezier curve")]
    public Transform startPoint;
    public Transform endPoint;
    public Transform startTangent;
    public Transform endTangent;

    public Vector2 CalculatePositionOverTime(float t)
    {
        //use a cubic Bezier formula
        // B[t] = ((1-t)^3)P[0] + 3((1-t)^2)(t)P[1] + 3(1-3)(t^2)P[2] + (t^3)P[3], 0 <= t <= 1
        return Mathf.Pow(1 - t, 3) * startPoint.position + 3 * Mathf.Pow(1 - t, 2) * t * startTangent.position + 3 * (1 - t) * Mathf.Pow(t, 2) * endTangent.position + Mathf.Pow(t, 3) * endPoint.position;
    }


    public Vector3 GetEndPosition()
    {
        return endPoint.position;
    }
}
