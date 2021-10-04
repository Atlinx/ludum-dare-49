using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores the current level data for loading on start
///
/// So when a level is loaded, this should already be set with the level data needed
/// </summary>
[CreateAssetMenu(fileName = "LevelCurrentData", menuName = "Level/LevelCurrentData")]
public class LevelCurrentData : ScriptableObject
{
    public LevelData levelData;
}
