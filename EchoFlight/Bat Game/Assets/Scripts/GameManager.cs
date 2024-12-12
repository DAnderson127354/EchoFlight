using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool gameIsPaused;

    public enum GameMode { Easy, Normal, Hard}
    public static GameMode gameMode;

    public static bool guidedModeOn = true;
    public static bool memoryModeOn = false;
    public static bool fullScoreModeOn = false;

    //Level Management
    public static Level[] levels = new Level[0];
    private static int currentLevel = 0;

    //Player Health system
    public static int playerLives = 3;

    public static void PauseGame()
    {
        gameIsPaused = true;
    }

    public static void UnPauseGame()
    {
        gameIsPaused = false;
    }

    public static void PlayerDeath()
    {
        playerLives--;

        if (playerLives < 0)
        {
            //Game over
            Debug.Log("Game over");
            //return to Title screen? or show custom Game Over menu
        }

    }

    /// <summary>
    /// Get the minimum target score needed to consider the level 100%
    /// </summary>
    /// <returns></returns>
    public static int GetCurrentLevelTargetScore()
    {
        return levels[currentLevel].baseScore;
    }

    public static int GetCurrentLevelDifficultyBonus()
    {
        return levels[currentLevel].difficultyRank switch
        {
            GameMode.Easy => 0,
            GameMode.Normal => 5,
            GameMode.Hard => 10,
            _ => 0,
        };
    }

    /// <summary>
    /// Get the amount awarded for successful memory mode completion
    /// </summary>
    /// <returns></returns>
    public static int GetCurrentLevelMemoryScoreBonus()
    {
        return levels[currentLevel].memoryModeBonus;
    }

    /// <summary>
    /// Get the name of the next level available
    /// </summary>
    /// <returns></returns>
    public static string GetNextLevel()
    {
        currentLevel++;

        if (currentLevel >= levels.Length)
        {
            //Game finished
            return "End"; //will change later
        }
        else
        {
            return levels[currentLevel].sceneName;
        }
        
    }

}
/// <summary>
/// Manage the base details of our levels
/// </summary>
[System.Serializable]
public class Level
{
    public string sceneName;
    public int baseScore;
    public int memoryModeBonus;
    public GameManager.GameMode difficultyRank;

}
