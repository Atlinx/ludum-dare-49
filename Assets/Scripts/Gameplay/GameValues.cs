using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "GameValues", menuName = "GameSO/GameValues")]
    public class GameValues : ScriptableObject
    {

        public struct JobData
        {
            public int JobAmount;
            public int ProductionPerJob;
        }
        
        public int PopulationLimit;
        public int PopulationTotal;
        public int PopulationJobTotal;

        public List<JobData> AgricultureProduction = new List<JobData>();
        public List<JobData> IndustryProduction = new List<JobData>();


        public int IndustryLimit;
        public int BaseIndustryLimit;
        public int IndustryTotal;
        public int PassiveIndustryIncome;
        
        public int FoodLimit;
        public int BaseFoodLimit;
        [FormerlySerializedAs("currentFoodTotal")] public int FoodTotal;

        public void Reset()
        {
            PopulationLimit = 0;
            PopulationTotal = 0;
            PopulationJobTotal = 0;
            IndustryLimit = 0;
            IndustryTotal = 0;
            BaseIndustryLimit = 0;
            FoodLimit = 0;
            FoodTotal = 0;
            BaseFoodLimit = 0;
        }
    }
}
