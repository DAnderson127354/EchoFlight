using UnityEngine;

/// <summary>
/// Point-giving collectable for the bat
/// </summary>
public class BugItem : MonoBehaviour
{
    public float points = 10f;

    /// <summary>
    /// Give points upon call
    /// Called on collision by the Player
    /// </summary>
    /// <returns></returns>
    public float EatBug()
    {
        gameObject.SetActive(false);
        return points;
    }

}
