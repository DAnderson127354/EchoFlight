using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MovesAlongRoute
{

    public delegate void OnPlayerRouteEnd();
    public static event OnPlayerRouteEnd PlayerReachesEnd;

    public delegate void OnPlayerCollision(float awardedPoints);
    public static event OnPlayerCollision PlayerCollision;


    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameIsPaused)
        {
            return;
        }

        //Player Movement
        if (time < 1 && routeIsSet)
        {
            MoveAlongRoute();

            if (time >= 1)
            {
                routeIsSet = false;
                PlayerReachesEnd?.Invoke();
            }
        }
    }

    private void OnDisable()
    {
        time = 0;
        routeIsSet = false;
    }


    //Collision Detection

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float awardedPointsForCollision = 0;
        //if collision is with bug
        if (collision.gameObject.TryGetComponent<BugItem>(out BugItem bugItem))
        {
            //earn points
            awardedPointsForCollision = bugItem.EatBug();
        }

        //Debug.Log(collision.gameObject.name);

        //if collision is with Obstacle
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            //remove life and respawn
            //if life reaches game over state exit game
            GameManager.PlayerDeath();

            awardedPointsForCollision = -100f;

            gameObject.SetActive(false);

        }

        PlayerCollision?.Invoke(awardedPointsForCollision);
        
    }

}
