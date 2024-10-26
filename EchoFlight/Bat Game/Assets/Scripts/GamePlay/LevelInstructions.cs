using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the requirements for completion of the level by processing the "instructions" the player slots in
/// </summary>
public class LevelInstructions : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;

    private int maxRoutesAllowed;

    [SerializeField]
    private Button goBttn;

    [Tooltip("Slots available and needed to use as instructions for how the player moves")]
    public Transform[] instructionSpots;

    [Tooltip("The possible bezier curves the player can use in the level. Reused and rotated for each route blueprint")]
    public BezierRoute[] routeTemplates;

    [HideInInspector]
    [Tooltip("Essentially a blueprint for what angle, direction and route type to use")]
    public RouteData[] myRoutes;

    private int currentRoute;

    [HideInInspector]
    public Coroutine routeInPlay;

    [Tooltip("A visual to aid the player in understanding what the direction piece they've selected does")]
    public GuideController guide;


    // Start is called before the first frame update
    void Start()
    {
        maxRoutesAllowed = instructionSpots.Length;
        currentRoute = 0;
        myRoutes = new RouteData[maxRoutesAllowed];

    }

    private void OnDisable()
    {
        PlayerController.PlayerReachesEnd -= CurrentRouteFinished;
    }

    /// <summary>
    /// Provide a demo of how the selected Directional Piece will make the bat do
    /// Does not run if the player is already moving
    /// </summary>
    /// <param name="instructions"></param>
    public void DemoRoute(RouteData instructions)
    {
        guide.gameObject.SetActive(true);
        routeTemplates[instructions.GetArrow()].transform.SetPositionAndRotation(player.transform.position, instructions.GetRotation());
        guide.SetCurrentRoute(routeTemplates[instructions.GetArrow()]);
    }

    /// <summary>
    /// Set a blueprint to one of the spaces available for this level. Route data can be swapped out
    /// </summary>
    /// <param name="route"></param>
    /// <param name="index"></param>
    public void SetRoute(RouteData route, int index)
    {
        myRoutes[index] = route;
    }

    /// <summary>
    /// When the player hits the 'Go' button, run through the available routes to make sure there are no gaps andto start the player movement if able
    /// </summary>
    public void AssembleRoutes()
    {
        if (myRoutes[0] != null)
        {
            routeTemplates[0].transform.SetPositionAndRotation(player.transform.position, myRoutes[0].GetRotation());

            for (int i = 1; i < myRoutes.Length; i++)
            {
                if (myRoutes[i] == null)
                {
                    //cannot run if there is a missing route
                    return;
                }
            }

            PlayerMove();
        }
    }

    /// <summary>
    /// Start player movement
    /// </summary>
    private void PlayerMove()
    {
        player.enabled = true;
        player.SetCurrentRoute(routeTemplates[currentRoute]);
        PlayerController.PlayerReachesEnd += CurrentRouteFinished;
        myRoutes[currentRoute].HighLightInstruction(true);
        goBttn.interactable = false;
    }

    /// <summary>
    /// End of the current route, so check conditions for next move
    /// </summary>
    public void CurrentRouteFinished()
    {
        myRoutes[currentRoute].HighLightInstruction(false);
        currentRoute += 1;

        //if this is the last route...
        if (currentRoute >= instructionSpots.Length)
        {
            currentRoute = 0;
            PlayerController.PlayerReachesEnd -= CurrentRouteFinished;
            //End Level and tally up score
        }
        else
        {
            myRoutes[currentRoute].HighLightInstruction(true);
            routeTemplates[myRoutes[currentRoute].GetArrow()].transform.SetPositionAndRotation(player.transform.position, myRoutes[currentRoute].GetRotation());
            player.SetCurrentRoute(routeTemplates[myRoutes[currentRoute].GetArrow()]);
        }
    }

    /// <summary>
    /// Check to see if our selected Direction Piece has made it over to one of our instruction spots
    /// </summary>
    /// <param name="piece"></param>
    /// <returns></returns>
    public bool CheckDirectionEndDragSpot(DirectionalPiece piece)
    {
        bool ans = false;

        int filledSpots = 0;
        for (int i = 0; i < instructionSpots.Length; i++)
        {
            if (CheckOverlap(piece.transform, instructionSpots[i]))
            {
                if (instructionSpots[i].transform.childCount > 0)
                {
                    //Swap out if something is already there
                    instructionSpots[i].GetChild(0).GetComponent<DirectionalPiece>().ResetPiece();
                }

                SetRoute(piece.route, i);

                piece.transform.position = instructionSpots[i].transform.position;
                piece.transform.SetParent(instructionSpots[i].transform);
                piece.SetSelectionOff();
                ans = true;
            }
            //at same time while we're already in a loop let's check how many are filled

            if (myRoutes[i] != null)
            {
                filledSpots++;
            }

        }

        goBttn.interactable = filledSpots == maxRoutesAllowed;

        return ans;
    }

    /// <summary>
    /// Determine if one UI transform is overlapping with another
    /// </summary>
    /// <param name="rect1"></param>
    /// <param name="rect2"></param>
    /// <returns></returns>
    private bool CheckOverlap(Transform rect1, Transform rect2)
    {
        //Get the position within the UI screen space
        Vector3 pos1 = Camera.main.WorldToScreenPoint(rect1.position);
        Vector3 pos2 = Camera.main.WorldToScreenPoint(rect2.position);

        //for at least one of the rects, we want to get the boundaries that the other rect's position must be between
        //we do this by getting half of the total width and height that we can then add to/subtract from the same rect's
        //position (which is in the middile of the UI image)
        float width = rect2.GetComponent<RectTransform>().rect.width / 2;
        float height = rect2.GetComponent<RectTransform>().rect.height / 2;

        if (pos1.x <= (pos2.x + width) && pos1.x >= (pos2.x - width))
        {
            if (pos1.y <= (pos2.y + height) && pos1.y >= (pos2.y - height))
            {
                //Debug.Log("within bounds!");
                return true;
            }
        }

        return false;
    }
}
