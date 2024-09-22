using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideController : MovesAlongRoute
{

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameIsPaused)
        {
            return;
        }

        if (time < 1)
        {
            MoveAlongRoute();

            if (time >= 1)
            {
                gameObject.SetActive(false);
            }
        }
        
    }
}
