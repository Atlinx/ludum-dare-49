using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

public class BuildingProduction : MonoBehaviour
{
    public enum ProductionType
    {
        Housing, Agriculture, Industry, None
    }

    public ProductionType currentProductionType;

    [Header("Housing")]
    public int HousingLimit;
    public float PerActivationPersonCreate;

    private float personActivationTracker;
    [SerializeField]
    private int currentPopulation;   
    [Space(4)]
    [Header("Agriculture")] 
    public int AgricultureProductionRate;
    public int AgricultureJobAmount;
    public int FoodStorageLimit;

    [Space(4)] 
    [Header("Industry")] 
    public int IndsutryProductionRate;
    public int IndustryJobAmount;
    public int IndustryStorageLimit;
    
    
    private BuildingBehaviour buildingBehaviour;

    private GameValues values;
    
    public void Init(BuildingBehaviour buildingBehaviour, GameValues values)
    {
        this.buildingBehaviour = buildingBehaviour;

        this.values = values;
        
        this.buildingBehaviour.OnTick += Activate;

    }

    public void Activate()
    {
        
        
        switch (currentProductionType)
        {
            case ProductionType.Housing:
                personActivationTracker++;

                if (personActivationTracker >= PerActivationPersonCreate)
                {
                    personActivationTracker = 0;
                    if (currentPopulation < HousingLimit) currentPopulation++;
                }

                if (values.FoodTotal >= currentPopulation)
                {
                    values.FoodTotal -= currentPopulation;
                }
                else
                {
                    currentPopulation--;
                    values.FoodTotal = 0;
                }

                values.PopulationTotal += this.currentPopulation;
                values.PopulationLimit += this.HousingLimit;
                
                break;
            case ProductionType.Agriculture:

                values.AgricultureProduction.Add(new GameValues.JobData()
                {
                    JobAmount = AgricultureJobAmount,
                    ProductionPerJob = AgricultureProductionRate
                });
                
                values.FoodLimit += FoodStorageLimit;
                
                break;
            case ProductionType.Industry:

                values.IndustryProduction.Add(new GameValues.JobData()
                {
                    JobAmount = IndustryJobAmount,
                    ProductionPerJob = IndsutryProductionRate
                });
                values.IndustryLimit += IndustryStorageLimit;
                break;
            case ProductionType.None:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
