using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EchoLocate : MonoBehaviour
{
    public float timeVisible = 1;

    private void Start()
    {
        GetComponent<Animator>().speed = (1/timeVisible);
    }

    public void Finish()
    {
        gameObject.SetActive(false);
    }
}
