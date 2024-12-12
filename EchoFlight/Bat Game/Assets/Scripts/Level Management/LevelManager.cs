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

    [Header("Final Results")]
    public GameObject endScreen;
    public Text finishStateTxt;
    public Text bugsTotalPointsTxt;
    public Text echoesTotalTxt;
    public Text difficultyBonusTxt;
    public Text memoryBonusTxt;
    public Text totalScoreTxt;

    public Button nextLevelBttn;
    public Button retryLevelBttn;

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

    /// <summary>
    /// Updates the display of our current stats
    /// </summary>
    /// <param name="bugAddition"></param>
    /// <param name="echoAddition"></param>
    public void UpdateStats(int bugAddition, int echoAddition)
    {
        bugsEaten += bugAddition;
        echosUsed += echoAddition;

        bugsEatenTxt.text = bugsEaten + "/" + bugList.Length;
        echoesUsedTxt.text = "" + echosUsed;

        levelScore += bugAddition * 10;
        totalPointsTxt.text = "Total: " + levelScore + "/" + targetScore;
    }

    /// <summary>
    /// Restarts the level
    /// </summary>
    public void Respawn()
    {
        //include slight delay here to showcase death animation when it is added
        //potentially, just call this during the animation and replace the spot this
        //is currently called with the animation call
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Function event called when echo is used
    /// </summary>
    public void EchoSent()
    {
        UpdateStats(0, 1);
        //need to add more to this to update points as well as check memory mode conditions

        if (memoryModeOn)
        {
            //show fail state
            PlayerDeath();
        }
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
            StartCoroutine(DisplayResults());
        }
    }

    IEnumerator DisplayResults()
    {
        endScreen.SetActive(true);
        for (int i = 0; i <= bugsEaten; i++)
        {
            bugsTotalPointsTxt.text = "Total Bugs Eaten: " + i;
            yield return new WaitForSeconds(0.25f);
        }

        for (int i = 0; i <= echosUsed; i++)
        {
            echoesTotalTxt.text = "Echoes used: " + i;
            yield return new WaitForSeconds(0.25f);
        }

        int diffBonus = GetCurrentLevelDifficultyBonus();
        difficultyBonusTxt.text = "Difficulty Bonus: +" + diffBonus;
        yield return new WaitForSeconds(0.25f);

        int memBonus = 0;
        if (echosUsed == 0)
        {
            memBonus = GetCurrentLevelMemoryScoreBonus();
        }
        memoryBonusTxt.text = "Memory Bonus: +" + memBonus;
        yield return new WaitForSeconds(0.25f);

        int total = 0;
        bugsTotalPointsTxt.text = "<i>Total Bugs Eaten: " + bugsEaten + "</i>";
        for (int i = 0; i <= bugsEaten * 10; i++)
        {
            totalScoreTxt.text = "Total Score: " + i;
            total++;
            yield return new WaitForSeconds(0.25f);
        }
        bugsTotalPointsTxt.text = "Total Bugs Eaten: " + bugsEaten;
        difficultyBonusTxt.text = "<i>Difficulty Bonus: +" + diffBonus + "</i>";
        total += diffBonus;
        totalScoreTxt.text = "Total Score: " + total;
        yield return new WaitForSeconds(0.25f);
        difficultyBonusTxt.text = "Difficulty Bonus: +" + diffBonus;
        memoryBonusTxt.text = "<i>Memory Bonus: +" + memBonus + "</i>";
        total += memBonus;
        totalScoreTxt.text = "Total Score: " + total;
        yield return new WaitForSeconds(0.25f);
        memoryBonusTxt.text = "Memory Bonus: +" + memBonus;


        if (fullScoreModeOn)
        {
            if (total < targetScore)
            {
                finishStateTxt.text = "Target not reached...";
                retryLevelBttn.gameObject.SetActive(true);
                yield break;
            }
        }

        finishStateTxt.text = "Level Complete!";
        nextLevelBttn.gameObject.SetActive(true);
    }


    public void NextLevel()
    {
        SceneManager.LoadScene(GetNextLevel());
    }

    public void ExitToTitle()
    {
        SceneManager.LoadScene("Title Screen");
    }

}
