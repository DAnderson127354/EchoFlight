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

    //Settings on the title screen

    public void SetGameMode(GameMode mode)
    {
        gameMode = mode;
    }

    public void ToggleGuidedMode(bool ans)
    {
        guidedModeOn = ans;
    }

    public void ToggleMemoryMode(bool ans)
    {
        memoryModeOn = ans;
    }

    public void ToggleFullScoreMode(bool ans)
    {
        fullScoreModeOn = ans;
    }

    public static void PlayerDeath()
    {
        playerLives--;

        if (playerLives < 0)
        {
            //Game over
            Debug.Log("Game over");
        }

    }

}
