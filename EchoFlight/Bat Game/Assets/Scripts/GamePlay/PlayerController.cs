using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MovesAlongRoute
{

    public delegate void OnPlayerRouteEnd();
    public static event OnPlayerRouteEnd PlayerReachesEnd;

    public delegate void OnPlayerCollision(float awardedPoints, CollisionType type);
    public static event OnPlayerCollision PlayerCollision;


    public enum CollisionType { Bug, Obstacle, Exit}

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
        CollisionType collisionType = CollisionType.Bug;
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
            collisionType = CollisionType.Obstacle;
           

            awardedPointsForCollision = -10f;

            gameObject.SetActive(false);

        }

        if (collision.gameObject.CompareTag("Exit"))
        {
            collisionType = CollisionType.Exit;

            //for future features, consider scenarios that if conditions are met, give extra points
            if (GameManager.memoryModeOn)
            {
                awardedPointsForCollision = GameManager.GetCurrentLevelMemoryScoreBonus();
            }
        }

        PlayerCollision?.Invoke(awardedPointsForCollision, collisionType);
        
    }

}
