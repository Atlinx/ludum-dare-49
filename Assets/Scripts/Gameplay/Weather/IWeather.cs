using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TargetData
{
    public List<BuildingProduction.ProductionType> productionTarget;
}

public abstract class IWeatherEffect : ScriptableObject
{
    public abstract ICollection<Vector2Int> GetTargetedSpots(TargetData data, WeatherController controller);

    public abstract GameObject GetWeatherSymbolObject();
}
