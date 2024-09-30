using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManager;

public class TitleScreen : MonoBehaviour
{
    [Tooltip("Base levels, includes both easy and medium")]
    public List<Level> levelList;
    [Tooltip("Extra levels for HardMode")]
    public List<Level> levelList_HardOnly;

    //Settings on the title screen
    public Toggle guideModeToggle;
    public Toggle memoryModeToggle; 
    public Toggle fullScoreModeToggle;

    /// <summary>
    /// Sets the base difficulty of the game
    /// </summary>
    /// <param name="mode"></param>
    public void SetGameMode(GameMode mode)
    {
        gameMode = mode;

        if (gameMode == GameMode.Hard)
        {
            guideModeToggle.isOn = false;
            guideModeToggle.interactable = false;

            memoryModeToggle.isOn = true;
            fullScoreModeToggle.isOn = true;
        }
        else
        {
            guideModeToggle.interactable = true;
            if (gameMode == GameMode.Easy)
            {
                guideModeToggle.isOn = true;

                memoryModeToggle.isOn = false;
                fullScoreModeToggle.isOn = false;
            }
        }
    }

    /// <summary>
    /// Enables/Disables the preview of what a directional piece will do. Enabled automatically in easy mode, unavailable in hard.
    /// Called on value change of guideModeToggle
    /// </summary>
    /// <param name="ans"></param>
    public void ToggleGuidedMode(bool ans)
    {
        guidedModeOn = ans;
    }

    /// <summary>
    /// Enables/Disables the extra challenge of Memory Mode, where extra echos are disabled, but players get more points as a reward
    /// Enabled automatically in hard mode. Disabled automatically (but still available) in easy mode.
    /// Called on value change of memoryModeToggle
    /// </summary>
    /// <param name="ans"></param>
    public void ToggleMemoryMode(bool ans)
    {
        memoryModeOn = ans;
    }

    /// <summary>
    /// Enabled/Disables the extra challenge of Full Score mode, where levels are only considered complete when every bug is eaten.
    /// Enabled automatically in hard mode. Disabled automatically (but still available) in easy mode.
    /// Called on value change of fullScoreModeToggle
    /// </summary>
    /// <param name="ans"></param>
    public void ToggleFullScoreMode(bool ans)
    {
        fullScoreModeOn = ans;
    }

    /// <summary>
    /// Begin the game by loading our first scene after setting our levels into our gamemanager for tracking
    /// Called by Play button
    /// </summary>
    public void StartGame()
    {
        if (gameMode == GameMode.Hard)
        {
            //include extra levels in Hard mode
            levels = new Level[levelList.Count + levelList_HardOnly.Count];
            levelList.CopyTo(levels);
            levelList_HardOnly.CopyTo(levels, levelList.Count-1);
        }
        else
        {
            levels = levelList.ToArray();
        }


        SceneManager.LoadScene(levels[0].sceneName);
    }

}
