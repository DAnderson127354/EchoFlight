using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierRoute))]
public class DrawBezierRoute : Editor
{
    void OnSceneGUI()
    {
        BezierRoute be = target as BezierRoute;

        be.startPoint.position = Handles.PositionHandle(be.startPoint.position, Quaternion.identity);
        be.endPoint.position = Handles.PositionHandle(be.endPoint.position, Quaternion.identity);
        be.startTangent.position = Handles.PositionHandle(be.startTangent.position, Quaternion.identity);
        be.endTangent.position = Handles.PositionHandle(be.endTangent.position, Quaternion.identity);

        // Visualize the tangent lines
        Handles.DrawDottedLine(be.startPoint.position, be.startTangent.position, 5);
        Handles.DrawDottedLine(be.endPoint.position, be.endTangent.position, 5);

        Handles.DrawBezier(be.startPoint.position, be.endPoint.position, be.startTangent.position, be.endTangent.position, Color.red, null, 5f);
    }
}
