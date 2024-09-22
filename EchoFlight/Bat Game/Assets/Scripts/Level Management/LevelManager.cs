using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public PlayerController player;
    private Vector2 playerSpawnPoint;

    [HideInInspector]
    public float levelScore = 100f;

    [Tooltip("List of all the bugs available in the level. Collecting all of them gets player 100%")]
    public BugItem[] bugList;

    [Tooltip("Track the number of times the player has used an echo-location beam")]
    [HideInInspector]
    public int echosUsed = 0;

    public LevelInstructions levelInstructions;
    private void Start()
    {
        playerSpawnPoint = player.transform.position;

        levelScore -= bugList.Length * 10;

        PlayerController.PlayerCollision += CollisionOccured;

        //display echolocation of whole level on start
    }

    public void Respawn()
    {
        //Debug.Log("Respawn");
        player.gameObject.SetActive(true);
        player.transform.position = playerSpawnPoint;
        levelScore = 100 - (bugList.Length * 10);

        foreach (BugItem bug in bugList)
        {
            bug.Respawn();
        }

        levelInstructions.enabled = true;
    }


    public void CollisionOccured(float points)
    {
        levelScore += points;

        if (levelScore <= 0)
        {
            //respawn
            levelInstructions.enabled = false;
            Respawn();
        }
    }

}
