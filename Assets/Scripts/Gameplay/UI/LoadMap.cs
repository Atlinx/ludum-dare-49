using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMap : MonoBehaviour
{
    public LevelCurrentData levelCurrentData;

    public LevelData levelData;
    
    public void OnClick()
    {
        levelCurrentData.levelData = levelData;
        SceneManager.LoadScene(1);
    }
}
