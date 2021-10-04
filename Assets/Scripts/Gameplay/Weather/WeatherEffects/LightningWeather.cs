using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "LightningWeather", menuName = "Weather/Lightning")]
public class LightningWeather :  IWeatherEffect
{
    

    public GameObject weatherSymbol;
    public override ICollection<Vector2Int> GetTargetedSpots(TargetData data, WeatherController controller)
    {
        int gridSizeX = controller.gameManager.buildingManager.gridSizeX;
        int gridSizeY = controller.gameManager.buildingManager.gridSizeY;


        Vector2Int randomGridPos = new Vector2Int(Random.Range(0, gridSizeX), Random.Range(0, gridSizeY));


        ICollection<Vector2Int> vector2Ints = new List<Vector2Int>();

        for (int coord = randomGridPos.x + randomGridPos.y * gridSizeY; coord < gridSizeX * gridSizeY; coord++)
        {
            int x = coord % gridSizeX;
            int y = coord / gridSizeY;

            
            var building = controller.gameManager.buildingManager.GetBuilding(new Vector2Int(x,y));
            
            if (building != null && data.productionTarget.Contains(building.production.currentProductionType))
            {
                
                vector2Ints.Add(new Vector2Int(x,y));
    
                return vector2Ints;
            }

            
        }
        for (int coord = 0; coord < randomGridPos.x + randomGridPos.y * gridSizeY; coord++)
        {
            int x = coord % gridSizeX;
            int y = coord / gridSizeY;

            var building = controller.gameManager.buildingManager.GetBuilding(new Vector2Int(x,y));

            if (building != null && data.productionTarget.Contains(building.production.currentProductionType))
            {
                
                vector2Ints.Add(new Vector2Int(x,y));
    
                return vector2Ints;
            }
        }
        
        vector2Ints.Add(randomGridPos);
        
        return vector2Ints;
    }

    public override GameObject GetWeatherSymbolObject()
    {
        return weatherSymbol;
    }

    public void Execute()
    {
        
    }
}
