using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gameplay;
using Gameplay.Buildings;
using UnityEngine;
using UnityEngine.Assertions;

public class BuildingManager : MonoBehaviour, IService
{
    // Events
    public delegate void BuildingEventHandler(BuildingBehaviour building);
    public BuildingEventHandler OnAddBuilding;
    public BuildingEventHandler OnRemoveBuilding;

    // Events
    public delegate void OnTickEventHandler();
    public OnTickEventHandler OnTick;

    public float ActivationTimePeriod = 1.0f;
    private float _activationTimeCurrent = 0.0f;

    // TODO: Refactor to use Vector2Int GridSize instead of gridSizeX and gridSizeY.
    public Vector2Int GridSize => new Vector2Int(gridSizeX, gridSizeY);
    public int gridSizeX, gridSizeY;

    public SokobanGrid SimulationBuildingGrid;
    public Grid2D<BuildingBehaviour> BuildingGrid => buildingGrid;
    private Grid2D<BuildingBehaviour> buildingGrid;
    
    public AudioSource movePieceSfx;

    public BuildingRegistry buildingsRegistry;

    public GameValues gameValues;

    // Move action queue. We are using a linked list since we need to be able to remove elements.
    public LinkedList<MoveBuildingAction> moveActionQueue = new LinkedList<MoveBuildingAction>();

    public SokobanGrid GenerateSimulationGrid(Grid2D<BuildingBehaviour> grid)
    {
        SokobanGrid simulationGrid = new SokobanGrid(gridSizeX, gridSizeY);
        for (int x = 0; x < grid.Size.x; x++)
            for (int y = 0; y < grid.Size.y; y++)
                simulationGrid.Set(x, y, BuildingToSokobanBlock(grid.Get(x, y)));
        return simulationGrid;
    }

    public void QueueMoveAction(MoveBuildingAction moveAction)
    {
        moveActionQueue.AddLast(moveAction);
    }

    public void DequeueMoveAction(MoveBuildingAction moveAction)
    {
        moveActionQueue.Remove(moveAction);
    }

    private void Awake()
    {
        ServiceLocator.Instance.AddService(this);
    }

    public void Init()
    {
        buildingGrid = new Grid2D<BuildingBehaviour>(gridSizeX, gridSizeY);
    }

    public void UpdateBuildings()
    {
        if (buildingGrid == null) return;
        
        _activationTimeCurrent += Time.deltaTime;

        bool currentlyTicked = _activationTimeCurrent >= ActivationTimePeriod;
        
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                var building = buildingGrid.Get(new Vector2Int(x, y));

                if (building == null) continue;
                
                building.BuildingUpdate();

                if (currentlyTicked)
                {
                    building.Tick();
                }
            }
        }

        if (currentlyTicked)
        {
            OnTick?.Invoke();
            if (moveActionQueue.Count > 0)
                movePieceSfx.Play();
            while (moveActionQueue.Count > 0)
            {
                moveActionQueue.First.Value.IsSuccessCallback.Invoke(MoveBuilding(moveActionQueue.First.Value));
                moveActionQueue.RemoveFirst();
            }
        }

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                var building = buildingGrid.Get(new Vector2Int(x, y));

                if (building == null) continue;

                if (currentlyTicked)
                    building.PostActivate();
            }
        }

        if (currentlyTicked)
        {
            _activationTimeCurrent = 0;
        }
    }

    public PushResult PushBuilding(Vector2Int position, Vector2Int direction, int amount)
    {
        SimulationBuildingGrid.MoveActionHistory.Clear();
        var pushResult = SimulationBuildingGrid.Push(position, direction, amount);
        while (SimulationBuildingGrid.MoveActionHistory.Count > 0)
        {
            var result = SimulationBuildingGrid.MoveActionHistory.Dequeue();
            MoveBuilding(result.StartPosition, result.EndPosition);
        }
        return pushResult;
    }

    public PushResult SimulatePushBuilding(Vector2Int position, Vector2Int direction, int amount)
    {
        return SimulationBuildingGrid.DeepCopy().Push(position, direction, amount);
    }

    public bool AddBuilding(BuildingData buildingData, int x, int y)
    {
        return AddBuilding(buildingData, x, y, true);
    }

    public bool AddBuilding(BuildingData buildingData, int x, int y, bool runOnPlace)
    {
        if (HasBuilding(x, y) || !WithinGrid(x, y))
        {
            return false;
        }


        var buildingObj = Instantiate(buildingData.building);

        buildingObj.transform.position = new Vector3(x, y, 0);

        BuildingBehaviour buildingBehaviourScript = buildingObj.GetComponent<BuildingBehaviour>();

        buildingBehaviourScript.Placed(x, y, buildingsRegistry.GetIndex(buildingData), this, runOnPlace);

        BuildingGrid.Set(new Vector2Int(x, y), buildingBehaviourScript);

        OnAddBuilding?.Invoke(buildingBehaviourScript);

        return true;
    }

    public bool AddBuilding(int buildingID, int x, int y)
    {
        var buildingReg = buildingsRegistry.Get(buildingID);

        return AddBuilding(buildingReg, x, y);
    }

    // TODO: Maybe refactor RemoveBuilding to return a BuildingBehaviour in order to 
    //       be more in line with other collection.Remove() methods?
    public bool RemoveBuilding(int x, int y)
    {
        if (!HasBuilding(x, y))
        {
            return false;
        }

        var buildingObj = BuildingGrid.Get(new Vector2Int(x, y));

        BuildingGrid.Set(new Vector2Int(x, y), null);

        buildingObj.Removed();

        return true;
    }

    public bool RemoveBuilding(Vector2Int position)
    {
        return RemoveBuilding(position.x, position.y);
    }


    // Helper functions

    public bool WillActivate()
    {
        return _activationTimeCurrent + Time.deltaTime >= ActivationTimePeriod;
    }

    /// <summary>
    /// Gets the building, true if building got, false otherwise;
    /// </summary>
    /// <param name="buildingBehaviour"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool GetBuilding(out BuildingBehaviour buildingBehaviour, int x, int y)
    {
        buildingBehaviour = null;
        if (!WithinGrid(x, y))
        {
            return false;
        }

        buildingBehaviour = buildingGrid.Get(x, y);

        return buildingBehaviour != null;
    }

    public bool GetBuilding(out BuildingBehaviour buildingBehaviour, Vector2Int position)
    {
        return GetBuilding(out buildingBehaviour, position.x, position.y);
    }

    /// <summary>
    /// Unchecked GetBuilding.
    /// </summary>
    /// <returns></returns>
    public BuildingBehaviour GetBuilding(Vector2Int position)
    {
        return buildingGrid.Get(position);
    }

    public bool HasBuilding(int x, int y)
    {
        if (!WithinGrid(x, y)) return false;

        if (buildingGrid.Get(x, y) != null) return true;

        return false;
    }

    public bool HasBuilding(Vector2Int position)
    {
        return HasBuilding(position.x, position.y);
    }

    /// <summary>
    /// Moves a building at originalPosition to newPosition if newPosition is not occupied.
    /// </summary>
    /// <param name="originalPosition"></param>
    /// <param name="newPosition"></param>
    /// <returns></returns>
    public bool MoveBuilding(Vector2Int originalPosition, Vector2Int newPosition)
    {
        return MoveBuildingHelper(originalPosition, newPosition, new HashSet<Vector2Int>());
    }

    private bool MoveBuildingHelper(Vector2Int originalPosition, Vector2Int newPosition, HashSet<Vector2Int> movedPositions)
    {
        var delta = newPosition - originalPosition;
        if (movedPositions.Contains(originalPosition))
            return true;
        movedPositions.Add(originalPosition);

        if (!CanMoveBuilding(originalPosition, newPosition))
            return false;

        var connectorBehaviour = buildingGrid.Get(originalPosition)?.GetComponent<ConnectorBehaviour>();
        if (connectorBehaviour != null)
        {
            foreach (var connectedBuilding in connectorBehaviour.ConnectedBuildings)
                if (!MoveBuildingHelper(connectedBuilding.Position, connectedBuilding.Position + delta, movedPositions))
                    return false;
        }

        buildingGrid.Set(newPosition, buildingGrid.Remove(originalPosition));
        buildingGrid.Get(newPosition).Position = newPosition;
        return true;

    }

    public bool MoveBuilding(MoveBuildingAction building)
    {
        return MoveBuilding(building.StartPosition, building.EndPosition);
    }

    public bool CanMoveBuilding(Vector2Int originalPosition, Vector2Int newPosition)
    {
        if (!(WithinGrid(originalPosition) || WithinGrid(newPosition)))
            return false;

        if (buildingGrid.Get(originalPosition) == null || buildingGrid.Get(newPosition) != null)
            return false;

        return true;
    }

    public bool WithinGrid(int x, int y)
    {
        return x < buildingGrid.Size.x && y < buildingGrid.Size.y && x >= 0 && y >= 0;
    }

    public bool WithinGrid(Vector2Int position)
    {
        return WithinGrid(position.x, position.y);
    }

    private SokobanGrid.Block BuildingToSokobanBlock(BuildingBehaviour building)
    {
        if (building == null)
            return null;

        var connector = building.GetComponent<ConnectorBehaviour>();
        if (connector != null)
            return new SokobanGrid.Connector(
                building.Movable,
                new HashSet<Vector2Int>(
                    connector.ConnectedBuildings.Select(x => x.Position)
                    ));
        else
            return new SokobanGrid.Block(building.Movable);
    }
}

public class MoveBuildingAction
{
    public MoveBuildingAction(Vector2Int startPosition, Vector2Int endPosition, Action<bool> isSuccessCallback)
    {
        StartPosition = startPosition;
        EndPosition = endPosition;
        IsSuccessCallback = isSuccessCallback;
    }

    public Vector2Int StartPosition { get; set; }
    public Vector2Int EndPosition { get; set; }
    public Action<bool> IsSuccessCallback { get; set; }
}