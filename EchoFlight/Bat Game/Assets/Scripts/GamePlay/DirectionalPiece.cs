using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Acts as a piece of instruction for the bat to follow as movement
/// Can be selected and dragged to fit in the interative fields of the UI
/// </summary>
public class DirectionalPiece : MonoBehaviour, ISelectHandler
{
    public LevelInstructions levelInstructionManager;
    public Transform canvasParent;

    [Header("Direction Visuals")]
    public Image arrowIcon;
    public Shadow arrowShadowEffect;
    public GameObject finishedCover;

    [Header("Direction Data")]
    public RouteData route;

    private Vector2 startSpot;
    private Transform startParent;

    private bool stillOn;
    private Vector2 startMouseSpot;

    private Vector2 pos;

    private Selectable selectableComp;

    // Start is called before the first frame update
    void Start()
    {
        route.SetUp(this);

        startSpot = transform.position;
        selectableComp = GetComponent<Selectable>();

        startParent = transform.parent;
    }

    public void OnSelect(BaseEventData eventData)
    {
        //Display visual of how the player would move
        if (GameManager.guidedModeOn)
        {
            levelInstructionManager.DemoRoute(route);
        }

        OnDragStart();
    }

    /// <summary>
    /// Gather information necessary to make sure we are actually dragging and not just clicking
    /// </summary>
    public void OnDragStart()
    {
        stillOn = true;
        
        startMouseSpot = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        StartCoroutine(StartDrag());
    }

    /// <summary>
    /// Beging the drag if we are still on the selected Piece
    /// </summary>
    /// <returns></returns>
    IEnumerator StartDrag()
    {

        yield return new WaitForSeconds(0.3f);

        //Check if the mouse has moved too far away
        if (Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), startMouseSpot) > 50f)
        {
            stillOn = false;
            EventSystem.current.SetSelectedGameObject(null);
        }

        //we have confirmed the intention is to drag
        if (stillOn)
        {
            transform.SetParent(canvasParent);
            StartCoroutine(WhileDragging());
        }
    }

    IEnumerator WhileDragging()
    {
        //for computer
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos;

        if (!Input.GetMouseButton(0))
        {
            EndDrag();
        }

        yield return new WaitForFixedUpdate();

        if (stillOn)
        {
            StartCoroutine(WhileDragging());
        }
    }

    /// <summary>
    /// We've released the mouse button, and so we need to check where we have dragged to and if it does anythign
    /// </summary>
    public void EndDrag()
    {
        stillOn = false;
       // Debug.Log("End drag");
        StopCoroutine(nameof(WhileDragging));

        EventSystem.current.SetSelectedGameObject(null);
        if (!levelInstructionManager.CheckDirectionEndDragSpot(this))
        {
            //return to start spot if not a successful overlap
            transform.SetParent(startParent);
            transform.position = startSpot;
        }
    }

    /// <summary>
    /// We don't want to interact with something already placed as we can swap things out if needed
    /// </summary>
    public void SetSelectionOff()
    {
        selectableComp.interactable = false;
    }

    /// <summary>
    /// Return to original section and make interactive again
    /// </summary>
    public void ResetPiece()
    {
        transform.SetParent(startParent);
        selectableComp.interactable = true;
    }

    /// <summary>
    /// Show a visual indicating that we are using the instructional data provided by this Piece in particular
    /// </summary>
    public void HighlightArrow()
    {
        arrowShadowEffect.enabled = true;
    }

    /// <summary>
    /// We have finished moving along the route determined by this Piece's data, so show a visual of this being done
    /// </summary>
    public void MarkAsFinished()
    {
        arrowShadowEffect.enabled = false;
        finishedCover.SetActive(true);
    }

}

/// <summary>
/// The data used to instruction the angle, direction and type of the bezier route
/// </summary>
[System.Serializable]
public class RouteData
{
    private DirectionalPiece attachedInstruction;

    public enum ArrowType
    {
        straight, diagonal, bent, curved
    }

    public ArrowType arrowType;

    public enum DirectionType
    {
        up, down, left, right
    }

    [Tooltip("The first direction the arrow begins to move to:")]
    public DirectionType directionType;

    /// <summary>
    /// Attach the UI half so we can activate visuals when associated data is being used
    /// </summary>
    /// <param name="attachment"></param>
    public void SetUp(DirectionalPiece attachment)
    {
        attachedInstruction = attachment;
    }

    /// <summary>
    /// Our type of arrow affects visuals and bezier curves
    /// </summary>
    /// <returns></returns>
    public int GetArrow()
    {
        return (int)arrowType;
    }

    /// <summary>
    /// Change the direction/rotation of a route to match the instructional data
    /// </summary>
    /// <returns></returns>
    public Quaternion GetRotation()
    {
        return directionType switch
        {
            DirectionType.up => Quaternion.Euler(0,0,90),
            DirectionType.down => Quaternion.Euler(0, 0, 270),
            DirectionType.left => Quaternion.Euler(0, 0, 180),
            DirectionType.right => Quaternion.Euler(0, 0, 0),
            _ => Quaternion.Euler(0, 0, 0),
        };
    }

    /// <summary>
    /// Turn off and on a visual showcasing use and finishing of the data
    /// </summary>
    /// <param name="highlight"></param>
    public void HighLightInstruction(bool highlight)
    {
        if (highlight)
        {
            attachedInstruction.HighlightArrow();
        }
        else
        {
            attachedInstruction.MarkAsFinished();
        }
    }

}
