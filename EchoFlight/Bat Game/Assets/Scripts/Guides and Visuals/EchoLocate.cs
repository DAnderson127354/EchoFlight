using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sends out an animated visual of a sprite mask to uncover hidden sprites along its path until it hits an impassable object
/// </summary>
[RequireComponent(typeof(Animator))]
public class EchoLocate : MonoBehaviour
{
    public float animationSpeed = 1;

    public LayerMask impassable;

    public Button echoLocateBttn;

    private void Start()
    {
        GetComponent<Animator>().speed = animationSpeed;
    }

    /// <summary>
    /// Called during the animation at the start so that players must wait for the echo to disappear before calling it again
    /// </summary>
    public void StartAnimation()
    {
        echoLocateBttn.interactable = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.name);
        if (((1 << collision.gameObject.layer) & impassable) != 0)
        {
            echoLocateBttn.interactable = true;
            gameObject.SetActive(false);
        }
    }
}
