using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Buildings;
using Unity.Mathematics;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public GameManager gameManager;
    public LevelPhaseManager levelPhaseManager;
    public bool spawnWeather = false;

    public IWeatherEffect requiredWeatherEffect;
    public TargetData targetData;

    public PrefabPool weatherWarningObjects;
    
    private struct WeatherGridData
    {
        public GameObject weatherWarningOverlay;
    }

    private Dictionary<Vector2Int,WeatherGridData> queuedWeatherEffects = new Dictionary<Vector2Int,WeatherGridData>();

    public void OnTick()
    {

        var currentPhase = levelPhaseManager.GetCurrentPhaseData();

        requiredWeatherEffect = currentPhase.weatherEffect;
        targetData.productionTarget = currentPhase.weatherProductionTargets;
        
        if (requiredWeatherEffect == null) return;
        
        // Incase the weather should wait a few before actually activated
        List<Vector2Int> activatedWeather = new List<Vector2Int>();
        
        if (queuedWeatherEffects.Count > 0)
        {
            foreach (var pair in queuedWeatherEffects)
            {
                var data = pair.Value;
                weatherWarningObjects.Put(data.weatherWarningOverlay);

                gameManager.buildingManager.RemoveBuilding(pair.Key);

                Instantiate(requiredWeatherEffect.GetWeatherSymbolObject()).transform.position = new Vector3(pair.Key.x, pair.Key.y, 0);
                
                activatedWeather.Add(pair.Key);

            }
        }

        foreach (var pos in activatedWeather)
        {
            queuedWeatherEffects.Remove(pos);
        }
        
        activatedWeather.Clear();
        
        if (CanSpawnWeather())
        {
            var weatherSpots = requiredWeatherEffect.GetTargetedSpots(targetData, this);
            foreach(var spots in weatherSpots)
            {
                var building = gameManager.buildingManager.GetBuilding(spots);
                if (building != null &&
                    building.production.currentProductionType == BuildingProduction.ProductionType.None)
                {
                    continue;
                }
                
                WeatherGridData data = new WeatherGridData();
                data.weatherWarningOverlay = weatherWarningObjects.Get();
                data.weatherWarningOverlay.transform.position = new Vector3(spots.x, spots.y, 0);
                queuedWeatherEffects.Add(spots, data);
            }
        }
        
    }


    public bool CanSpawnWeather()
    {
        return spawnWeather && levelPhaseManager.currentRemainingTicksUntilNextState > 1;
    }
}
