using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reach a certain stat
/// </summary>

[CreateAssetMenu(fileName =  "StatObjective", menuName = "LevelObjectives/Stat")]
public class StatObjective : LevelObjective
{

    public enum StatType
    {
        SCORE_POPULATION_TOTAL, SCORE_ARGICULTURE_TOTAL, SCORE_INDUSTRY_TOTAL, BUILDING_HOUSE_TOTAL, BUILDING_AGRICULTURE_TOTAL, BUILDING_INDUSTRY_TOTAL
    }
    
    [Serializable]
    public struct Stat
    {
        public StatType type;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool TestCompletion()
    {
        throw new NotImplementedException();
    }

    public override string[] GetDescription()
    {
        throw new NotImplementedException();
    }
}
