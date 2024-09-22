using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugItem : MonoBehaviour
{
    public float points = 10f;

    public float EatBug()
    {
        gameObject.SetActive(false);
        return points;
    }

    public void Respawn()
    {
        gameObject.SetActive(true);
    }

}
