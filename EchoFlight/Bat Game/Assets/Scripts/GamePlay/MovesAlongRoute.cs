using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovesAlongRoute : MonoBehaviour
{
    protected float time = 0;

    protected BezierRoute currentRoute;

    protected bool routeIsSet = false;

    public void SetCurrentRoute(BezierRoute newRoute)
    {
        currentRoute = newRoute;
        routeIsSet = true;
        time = 0;
    }

    protected void MoveAlongRoute()
    {
        time += Time.deltaTime;

        transform.position = currentRoute.CalculatePositionOverTime(time);
    }
}
