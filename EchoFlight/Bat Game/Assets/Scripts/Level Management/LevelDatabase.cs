using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates an asset containing references to pre-set levels
/// </summary>
[System.Serializable]
[CreateAssetMenu(menuName = "Level_Database", fileName = "New Database")]
public class LevelDatabase : ScriptableObject
{
    public List<Level> levels;
}
