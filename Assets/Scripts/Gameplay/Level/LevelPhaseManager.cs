using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LevelPhaseManager : MonoBehaviour
{

    public GameManager gameManager;


    public UnityEvent BuildPhaseStartEvent;
    public UnityEvent BuildPhaseTickEvent;
    public UnityEvent WeatherPhaseStartEvent;
    public UnityEvent WeatherPhaseTickEvent;
    public UnityEvent FinishEvent;

    public ProgressBarUI progressUntilNextTickUI;
    public TextMeshProUGUI ticksRemainProgressText;
    
    
    private LevelData levelData;
    
    private int CurrentPhaseIndex = 0;

    /// <summary>
    /// Default objective will be just to survive
    /// </summary>
    private LevelObjective currentObjective = null;

    private int ObjectivePhaseIndex = 0;

    private int tickCounter = 0;
    public int currentRemainingTicksUntilNextState { get; private set; }
    private float updateTimer = 0;
    
    
    public void SetLevelData(LevelData data)
    {
        levelData = data;
        FetchNextObjective();
        ActivateBuildPhase();
        
        
        var currentPhase = levelData.phaseQueue[CurrentPhaseIndex];
        ticksRemainProgressText.text = currentPhase.BuildTime.ToString();
        updateTimer = gameManager.buildingManager.ActivationTimePeriod;
    }

    private void Update()
    {
        updateTimer -= Time.deltaTime;
        progressUntilNextTickUI.currentValue = gameManager.buildingManager.ActivationTimePeriod - updateTimer;
        progressUntilNextTickUI.maxValue = gameManager.buildingManager.ActivationTimePeriod;
        progressUntilNextTickUI.UpdateBar();
    }

    public void OnTick()
    {
        var currentPhase = levelData.phaseQueue[CurrentPhaseIndex];
        updateTimer = gameManager.buildingManager.ActivationTimePeriod;

        tickCounter++;
        
        //Debug.Log("Current phase - " + currentPhase );
        //Debug.Log("Current tick- " + tickCounter );


        currentRemainingTicksUntilNextState = GetRemainingTurnsUntilNextState();        
        ticksRemainProgressText.text = currentRemainingTicksUntilNextState.ToString();

        if (IsWeatherPhase())
        {
            WeatherPhaseTickEvent.Invoke();
        }
        else
        {
            BuildPhaseTickEvent.Invoke();
        }
        
        if (currentPhase.BuildTime + currentPhase.WeatherTime <= tickCounter)
        {
            //Phase done
            var completed = currentObjective == null || currentObjective.TestCompletion();
            
            
            
            // If we are the last phase, and no objective, we won
            if (CurrentPhaseIndex == levelData.phaseQueue.Count - 1 && completed)
            {
                gameManager.GameWon();
                Finish();
                return;
            }else if (currentObjective != null && completed)
            {
                CurrentPhaseIndex = ObjectivePhaseIndex + 1;

                if (CurrentPhaseIndex == levelData.phaseQueue.Count)
                {
                    Finish();
                    return;
                }
                
                FetchNextObjective();
            }
            else
            {
                if (currentPhase.reapetUntilObjectiveFinished == null)
                {
                    CurrentPhaseIndex++;
                }
                else
                {
                    CurrentPhaseIndex = Mathf.Max(0, CurrentPhaseIndex - currentPhase.repeatPreviousAmount);
                }
            }
            
            tickCounter = 0;
            ActivateBuildPhase();
        }
        else if(currentPhase.BuildTime == tickCounter)
        {
            ActivateWeatherPhase();
        }


        if (CurrentPhaseIndex < levelData.phaseQueue.Count)
        {
            currentPhase = levelData.phaseQueue[CurrentPhaseIndex];
            if (tickCounter == currentPhase.BuildTime + currentPhase.WeatherTime - 1)
            {
                //ticksRemainProgressText.text = (currentPhase.BuildTime).ToString();
            }
        }
    }

    public void ActivateBuildPhase()
    {
        if (CurrentPhaseIndex < levelData.phaseQueue.Count)
        {
            var currentPhase = levelData.phaseQueue[CurrentPhaseIndex];
            ticksRemainProgressText.text = (currentPhase.BuildTime).ToString();
        }

        //Debug.Log("Start build phase");
        if(BuildPhaseStartEvent != null) BuildPhaseStartEvent.Invoke();
    }

    public void ActivateWeatherPhase()
    {
        //Debug.Log("Start weather phase");
        if(WeatherPhaseStartEvent != null)  WeatherPhaseStartEvent?.Invoke();
    }
    
    public void Finish()
    {
        //Debug.Log("Finished phases and level");
        if(FinishEvent != null) FinishEvent?.Invoke();
    }
    
    public String[] GetCurrentObjectiveDescription()
    {
        if (currentObjective == null)
        {
            return new[] { "Survive"};
        }

        return currentObjective.GetDescription();
    }

    private int GetRemainingTurnsUntilNextState()
    {
        var currentPhase = levelData.phaseQueue[CurrentPhaseIndex];
        if (tickCounter > currentPhase.BuildTime)
        {
            return currentPhase.WeatherTime - (tickCounter - currentPhase.BuildTime);
        }
        else if (tickCounter == currentPhase.BuildTime)
        {
            return (currentPhase.WeatherTime);
        }
        else if (tickCounter == currentPhase.BuildTime + currentPhase.WeatherTime)
        {
            return (currentPhase.BuildTime);
        }
        else
        {
            return currentPhase.BuildTime - tickCounter;
        }
    }

    public LevelData.PhaseData GetCurrentPhaseData()
    {
        return levelData.phaseQueue[CurrentPhaseIndex];
    }
    
    private void FetchNextObjective()
    {
        for (int i = ObjectivePhaseIndex; i < levelData.phaseQueue.Count; i++)
        {
            var phaseData = levelData.phaseQueue[i];

            if (phaseData.reapetUntilObjectiveFinished != null)
            {
                currentObjective = phaseData.reapetUntilObjectiveFinished;
                ObjectivePhaseIndex = i;
                return;
            }
        }

        currentObjective = null;
        ObjectivePhaseIndex = levelData.phaseQueue.Count - 1;
    }

    private bool IsWeatherPhase()
    {
        var currentPhase = levelData.phaseQueue[CurrentPhaseIndex];
        return tickCounter > currentPhase.BuildTime;
    }
    
}
