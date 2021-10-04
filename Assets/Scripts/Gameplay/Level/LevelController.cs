using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEditor.UIElements;
using UnityEngine;

/// <summary>
/// A level controller will control the whole lifespan of a level, from starting to finishing
/// </summary>
public class LevelController : MonoBehaviour
{
    public GameManager gameManager;
    public BuildingManager buildingManager;
    public LevelPhaseManager levelPhaseManager;
    public ShopManager shopManager;
    public WeatherController weatherController;
    
    public GameObject backgroundLayer;

    public LevelCurrentData levelCurrentData;

    public LevelBackgroundBuilder backgroundBuilder;
    private LevelData levelData;
    
    
    // Start is called before the first frame update
    void Start()
    {
        levelData = levelCurrentData.levelData;
        
        levelPhaseManager.SetLevelData(levelData);
        
        buildingManager.gridSizeX = levelData.sizeX;
        buildingManager.gridSizeY = levelData.sizeY;
        
        gameManager.gameValues.Reset();
        gameManager.gameValues.BaseFoodLimit = levelData.BaseFoodLimit;
        gameManager.gameValues.BaseIndustryLimit = levelData.BaseIndustryLimit;
        gameManager.gameValues.IndustryTotal = levelData.StartIndustrySupply;
        gameManager.gameValues.FoodTotal = levelData.StartFoodSupply;
        gameManager.gameValues.PassiveIndustryIncome = levelData.PassiveIndustryIncome;
        gameManager.Init();
        buildingManager.Init();
        
        gameManager.GameTickEvent.AddListener(levelPhaseManager.OnTick);

        BuildMap();

        Vector3 centerGridWorldPos  = new Vector3(levelData.sizeX/2, levelData.sizeY/2, -10);
        if (levelData.sizeX % 2 == 0)
        {
            centerGridWorldPos.x -= 0.5f;
        }

        if (levelData.sizeY % 2 == 0)
        {
            centerGridWorldPos.y -= 0.5f;
        }
        
        
        Camera.main.transform.position = centerGridWorldPos;
        //Camera.main.orthographicSize = Mathf.Max(levelData.sizeY, levelData.sizeX) * (6.805583f / 10);

        centerGridWorldPos.z = 0;
        backgroundLayer.transform.position = centerGridWorldPos;
        backgroundLayer.transform.localScale = new Vector3(levelData.sizeX, levelData.sizeY);
        Canvas.ForceUpdateCanvases();
        
        shopManager.Init();
        backgroundBuilder.CreateBackground(levelData.sizeX, levelData.sizeY);
    }


    private void BuildMap()
    {
        string map = levelData.mapParse;
        string[] mapData = map.Split(' ', '\n');

        if (mapData.Length != levelData.sizeY * levelData.sizeX)
        {
            Debug.LogError("Map data count does not match map size " + mapData.Length + " " + levelData.sizeX * levelData.sizeY);
            return;
        }

        for (int i = 0; i < mapData.Length; i++)
        {
            Vector2Int currentPos = new Vector2Int(i % levelData.sizeX,
                levelData.sizeY - (i / levelData.sizeX) - 1);
            
            
            int buildingIndex = int.Parse(mapData[i]) - 1;

            if (buildingIndex < -1 || buildingIndex >= levelData.buildingDataList.Count)
            {
                Debug.LogError("Detected char is out of bounds - "  + buildingIndex);
                return;
            }

            if (buildingIndex >= 0)
            {
                var building = levelData.buildingDataList[buildingIndex];
                buildingManager.AddBuilding(building, currentPos.x, currentPos.y, false);
            }

        }


    }

}
