using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public class LevelManager : MonoBehaviour
{
    public PlayerController player;

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
        levelScore = GetCurrentLevelTargetScore();
        levelScore -= bugList.Length * 10;

        PlayerController.PlayerCollision += CollisionOccured;

        //display echolocation of whole level on start
    }

    public void Respawn()
    {
        //include slight delay here to showcase death animation when it is added
        //potentially, just call this during the animation and replace the spot this
        //is currently called with the animation call
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    /// <summary>
    /// Add points based on collision or potentially respawn
    /// </summary>
    /// <param name="points"></param>
    public void CollisionOccured(float points, PlayerController.CollisionType collision)
    {
        levelScore += points;

        if (collision == PlayerController.CollisionType.Obstacle)
        {
            //remove life and respawn
            //if life reaches game over state exit game
            PlayerDeath();

            //respawn
            Respawn();
        }
        else if (collision == PlayerController.CollisionType.Exit)
        {
            //Exit level
            int target = GetCurrentLevelTargetScore();

            if (memoryModeOn)
            {
                target += GetCurrentLevelMemoryScoreBonus();
            }

            //Add in display showcasing score and success states

            if (fullScoreModeOn)
            {

                if (levelScore >= target)
                {
                    //Display screen that has button with functionality of:
                    //NextLevel();
                    //or
                    //Load Title Screen
                }
                else
                {
                    //fail state for full score mode
                    //Display screen that has button with functionality of:
                    //PlayerDeath();
                    //Respawn();
                    //or
                    //Load Title Screen
                }

            }
            else
            {
                //Display screen that has button with functionality of:
                //NextLevel();
                //or
                //Load Title Screen
            }
        }
    }

}
