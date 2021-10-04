using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(BuildingProduction))]
public class BuildingBehaviour : MonoBehaviour
{
    // TODO: Rename events to OnEvent
    // TODO: Rename delegates to DelegateNameEventHandler
    // TODO: Rename Activate to Tick.
    public BuildingManager BuildingManager { get; set; }
    public BuildingProduction production;
    
    public delegate void PlacedEventHandler();
    public PlacedEventHandler OnPlaced;
    
    public delegate void RemovedEventHandler();
    public RemovedEventHandler OnRemove;
    
    public delegate void TickEventHandler();
    public TickEventHandler OnTick;

    public delegate void PostTickEventHandler();
    public PostTickEventHandler OnPostTick;

    public delegate void UpdateEventHandler();
    public UpdateEventHandler OnUpdate;
    
    // Do not update Position if you want to move a building.
    // Instead, move a building with BuildingManager.MoveBuilding();
    public Vector2Int Position 
    {
        get => position;
        set
        {
            position = value;
            transform.position = (Vector2) position;
        }
    }
    private Vector2Int position;
    public bool IsMoving { get; set; } = false;

    public bool Movable { get; set; } = true;

    public int regID { get; private set; }
    


    // Invokes
    public void Tick()
    {
     
        OnTick?.Invoke();
    }

    public void PostActivate()
    {
        OnPostTick?.Invoke();
    }

    public void BuildingUpdate()
    {
        
        OnUpdate?.Invoke();
    }


    public void Placed(int x, int y, int regID, BuildingManager buildingManager, bool runOnPlace)
    {
        Position = new Vector2Int(x, y);
        this.regID = regID;
        this.BuildingManager = buildingManager;
        
        production = GetComponent<BuildingProduction>();
        production.Init(this, buildingManager.gameValues);
        if (runOnPlace)
            OnPlaced?.Invoke();
    }
    
    public void Removed(){
        OnRemove?.Invoke();
        Destroy(this.gameObject);
    }
    
    
    // Getters Setters
    
    
    

}
