using UnityEngine;

/// <summary>
/// Manages the actions, collisions and movement of the Player
/// </summary>
public class PlayerController : MovesAlongRoute
{

    public delegate void OnPlayerRouteEnd();
    public static event OnPlayerRouteEnd PlayerReachesEnd;

    public delegate void OnPlayerCollision(float awardedPoints, CollisionType type);
    public static event OnPlayerCollision PlayerCollision;

    public delegate void OnEchoLocation();
    public static event OnEchoLocation EchoLocateCalled;

    public enum CollisionType { Bug, Obstacle, Exit}

    public GameObject beamRoute;
    public Rigidbody2D echoBeam;
    public Vector2 beamForce;

    private bool echoLocateOn = false;

    //what is the lowest Y point until mouse is over bottom UI
    public float screenEdgeY;
    //what is the farthest X point until mouse is over right UI
    public float screenEdgeX;

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

        if (echoLocateOn)
        {
            //display projectile route
            
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 targetPoint = new(transform.position.x - mousePos.x, mousePos.y - transform.position.y, 0);
            float angle = Mathf.Atan2(targetPoint.x, targetPoint.y) * Mathf.Rad2Deg;

            beamRoute.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 0, angle));

            //Debug.Log(mousePos);
            if (Input.GetMouseButton(0) && (mousePos.y > screenEdgeY && mousePos.x < screenEdgeX))
            {
                EchoLocate(angle);
            }
        }

    }

    private void EchoLocate(float angle)
    {
        if (GameManager.memoryModeOn)
        {
            //fail condition
            GameManager.memoryModeOn = false;
        }

        beamRoute.SetActive(false);
        echoBeam.gameObject.SetActive(true);
        echoBeam.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 0, angle + 45));

        echoBeam.AddForce(echoBeam.transform.rotation * beamForce, ForceMode2D.Impulse);
        echoLocateOn = false;

        EchoLocateCalled?.Invoke();
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

    /// <summary>
    /// Called by the EchoLocate button
    /// Turns on echo location projection if not on, and turns off if it is
    /// </summary>
    public void ToggleEcho()
    {
        echoLocateOn = !echoLocateOn;
        beamRoute.SetActive(echoLocateOn);
    }


}
