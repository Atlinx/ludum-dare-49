using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Buildings;
using JetBrains.Annotations;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelData", menuName = "Level/LevelData")]
public class LevelData : ScriptableObject
{
    [Serializable]
    public struct PhaseData
    {
        public int BuildTime;
        public int WeatherTime;

        public IWeatherEffect weatherEffect;
        public List<BuildingProduction.ProductionType> weatherProductionTargets;
        
        /// <summary>
        /// Repeat to the past x phases;
        /// </summary>
        //[Header("Wait for Objective finish")]
        [HideInInspector]
        public LevelObjective reapetUntilObjectiveFinished;
        [HideInInspector]
        public int repeatPreviousAmount;

    }
    
    public int sizeX, sizeY;

    public int StartFoodSupply;
    public int BaseFoodLimit;
    public int StartIndustrySupply;
    public int BaseIndustryLimit;
    public int PassiveIndustryIncome;
    
    
    
    public List<PhaseData> phaseQueue = new List<PhaseData>();
    
    [Header("Map data (MAP DATA - 1, 0 = none, 1 = index 0)")]
    public List<BuildingData> buildingDataList = new List<BuildingData>();
    
    [TextArea(2, 100)]
    public String mapParse;

}
