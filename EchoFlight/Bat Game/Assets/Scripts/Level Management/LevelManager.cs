using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManager;
/// <summary>
/// Manages the content and scoring of each level
/// </summary>
public class LevelManager : MonoBehaviour
{
    public PlayerController player;
    public LevelInstructions levelInstructions;

    [HideInInspector]
    public float levelScore = 100f;
    private float targetScore;

    [Tooltip("List of all the bugs available in the level. Collecting all of them gets player 100%")]
    public BugItem[] bugList;
    private int bugsEaten = 0;

    [Tooltip("Track the number of times the player has used an echo-location beam")]
    [HideInInspector]
    public int echosUsed = 0;

    [Header("Stats")]
    public Text bugsEatenTxt;
    public Text echoesUsedTxt;
    public Text totalPointsTxt;

    

#if UNITY_EDITOR
    [Header("In-Editor Run Only")]
    [Tooltip("For testing purposes, load in the desired level database")]
    public LevelDatabase inUse_DB;
#endif


    private void Start()
    {
#if UNITY_EDITOR
        //if we are just running a level in Unity, we'll go ahead and fill in our level list with a quick grab to a database of our choice
        if (levels.Length == 0)
        {
            levels = inUse_DB.levels.ToArray();
        }

#endif

        targetScore = GetCurrentLevelTargetScore();
        levelScore = targetScore - (bugList.Length * 10);
        UpdateStats(0, 0);

        PlayerController.PlayerCollision += CollisionOccured;
        PlayerController.EchoLocateCalled += EchoSent;

        //display echolocation of whole level on start
        //show bat animation

    }

    private void OnDisable()
    {
        PlayerController.PlayerCollision -= CollisionOccured;
        PlayerController.EchoLocateCalled -= EchoSent;
    }

    public void UpdateStats(int bugAddition, int echoAddition)
    {
        bugsEaten += bugAddition;
        echosUsed += echoAddition;

        bugsEatenTxt.text = "Bugs Eaten: " + bugsEaten + "/" + bugList.Length;
        echoesUsedTxt.text = "Echoes Used: " + echosUsed;

        totalPointsTxt.text = "Total Points: " + levelScore + "/" + targetScore;
    }


    public void Respawn()
    {
        //include slight delay here to showcase death animation when it is added
        //potentially, just call this during the animation and replace the spot this
        //is currently called with the animation call
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EchoSent()
    {
        UpdateStats(0, 1);
        //need to add more to this to update points as well as check memory mode conditions
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
        else if (collision == PlayerController.CollisionType.Bug)
        {
            UpdateStats(1, 0);
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
