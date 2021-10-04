using System.Collections;
using System.Collections.Generic;
using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public BuildingManager buildingManager;

    public GameValues gameValues;

    public StatsUI statsUI;

    public UnityEvent GameTickEvent;

    public GameObject lostScreen;
    public GameObject wonScreen;
    public TextMeshProUGUI buildingAmountText;
    
    private bool CanLose = false;
    private bool Lost = false;
    private bool Won = false;

    public UnityEvent GameWonEvent;
    
    private float cooldownAfterLostWin = 0;
    
    // Start is called before the first frame update
    public void Init()
    {
        gameValues.FoodLimit = gameValues.BaseFoodLimit;
        gameValues.IndustryLimit = gameValues.BaseIndustryLimit;
        statsUI.SetPopulationText(gameValues.PopulationTotal, 0, gameValues.PopulationLimit);
        statsUI.SetFoodText(gameValues.FoodTotal, 0, gameValues.FoodLimit);
        statsUI.SetIndustryText(gameValues.IndustryTotal, 0, gameValues.IndustryLimit);
    }

    // Update is called once per frame
    void Update()
    {
        if (Lost || Won)
        {
            cooldownAfterLostWin += Time.deltaTime;

            if (cooldownAfterLostWin > 1.0f && Input.GetKeyDown(KeyCode.Mouse0))
            {
                SceneManager.LoadScene(2);
            }
            
            return;
        }
        if (!buildingManager.WillActivate())
        {
            buildingManager.UpdateBuildings();
            return;
        }
        
        var lastPopulationTotal = gameValues.PopulationTotal;
        var lastFoodTotal = gameValues.FoodTotal;
        var lastIndustryTotal = gameValues.IndustryTotal;
        
        gameValues.PopulationTotal = 0;
        gameValues.FoodLimit = gameValues.BaseFoodLimit;

        gameValues.PopulationLimit = 0;
        gameValues.IndustryLimit = gameValues.BaseIndustryLimit;
        
        gameValues.AgricultureProduction.Clear();
        gameValues.IndustryProduction.Clear();
        
        
        buildingManager.UpdateBuildings();

        gameValues.PopulationJobTotal = gameValues.PopulationTotal;

        
        
        //Calculate Industry Total
        foreach(var job in gameValues.IndustryProduction)
        {
            if (gameValues.PopulationJobTotal <= 0) continue; 
            
            if (gameValues.PopulationJobTotal >= job.JobAmount)
            {
                gameValues.IndustryTotal += job.ProductionPerJob * job.JobAmount;
                gameValues.PopulationJobTotal -= job.JobAmount;
            }else if (gameValues.PopulationJobTotal < job.JobAmount)
            {
                gameValues.IndustryTotal += gameValues.PopulationJobTotal * job.ProductionPerJob;
                gameValues.PopulationJobTotal = 0;
            }
        }
        
        //Calculate Food Total
        foreach(var job in gameValues.AgricultureProduction)
        {
            if (gameValues.PopulationJobTotal <= 0) continue; 
            
            if (gameValues.PopulationJobTotal >= job.JobAmount)
            {
                gameValues.FoodTotal += job.ProductionPerJob * job.JobAmount;
                gameValues.PopulationJobTotal -= job.JobAmount;
            }else if (gameValues.PopulationJobTotal < job.JobAmount)
            {
                gameValues.FoodTotal += gameValues.PopulationJobTotal * job.ProductionPerJob;
                gameValues.PopulationJobTotal = 0;
            }
        }

        gameValues.IndustryTotal += gameValues.PassiveIndustryIncome;
        
        var populationDifference = gameValues.PopulationTotal - lastPopulationTotal;
        var foodDifference = gameValues.FoodTotal - lastFoodTotal;
        var industryDifference = gameValues.IndustryTotal - lastIndustryTotal;
        
        
        
        if (gameValues.FoodTotal > gameValues.FoodLimit)
        {
            gameValues.FoodTotal = gameValues.FoodLimit;
        }

        if (gameValues.IndustryTotal > gameValues.IndustryLimit)
        {
            gameValues.IndustryTotal = gameValues.IndustryLimit;
        }
        
        
        statsUI.SetPopulationText(gameValues.PopulationTotal, populationDifference, gameValues.PopulationLimit);
        statsUI.SetFoodText(gameValues.FoodTotal, foodDifference, gameValues.FoodLimit);
        statsUI.SetIndustryText(gameValues.IndustryTotal, industryDifference, gameValues.IndustryLimit);

        if (CanLose)
        {
            if (gameValues.PopulationTotal == 0)
            {
                Debug.Log("GAME LOST");
                Lost = true;
                lostScreen.SetActive(true);
                return;
            }
        }
        if (gameValues.PopulationTotal > 0)
        {
            CanLose = true;
        }
        
        
        GameTickEvent?.Invoke();
        
    }


    public void GameWon()
    {
        int buildingsCount = 0;

        for (int x = 0; x < buildingManager.gridSizeX; x++)
        {
            for (int y = 0; y < buildingManager.gridSizeY; y++)
            {
                var building = buildingManager.GetBuilding(new Vector2Int(x, y));
                if (building != null &&
                    building.production.currentProductionType != BuildingProduction.ProductionType.None)
                {
                    buildingsCount++;
                }
            }
        }
        
        
        Won = true;
        GameWonEvent?.Invoke();
        wonScreen.SetActive(true);
        buildingAmountText.text = "Buildings " + buildingsCount;
    }
}
