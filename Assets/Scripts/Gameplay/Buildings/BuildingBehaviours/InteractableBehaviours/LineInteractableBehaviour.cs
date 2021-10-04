using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LineInteractableBehaviour: MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private Vector2Int delta;

    [Header("Dependencies")]
    [SerializeField]
    private BuildingBehaviour buildingBehaviour;
    [SerializeField]
    private InteractableBehaviour interactableBehaviour;
    
    private BuildingManager buildingManager;
    
    private void Awake()
    {
        buildingManager = ServiceLocator.Instance.GetService<BuildingManager>();

        interactableBehaviour.OnGetInteractablePositions.AddListener(OnGetInteractablePositions);
    }

    private void OnGetInteractablePositions(HashSet<Vector2Int> positions)
    {
        Vector2Int currPos = buildingBehaviour.Position + delta;
        while (buildingManager.BuildingGrid.WithinGrid(currPos) && !buildingManager.HasBuilding(currPos))
        {
            positions.Add(currPos);
            currPos += delta;
        }
    }
}