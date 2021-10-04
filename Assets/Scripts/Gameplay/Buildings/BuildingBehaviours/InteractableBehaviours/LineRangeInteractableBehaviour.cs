using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Returns grid positions in a range in the direction of a building's position.
/// For example, a range of (5, 10) will return all grid positions in the direction
/// of the building that are anywhere from 5-10 blocks away from the building.
/// </summary>
public class LineRangeInteractableBehaviour : MonoBehaviour
{
    public Vector2Int range;

    [Header("Dependencies")]
    [SerializeField]
    private InteractableBehaviour interactableBehaviour;
    [SerializeField]
    private DirectionBehaviour directionBehaviour;
    [SerializeField]
    private BuildingBehaviour buildingBehaviour;
    
    private BuildingManager buildingManager;

    private void Awake()
    {
        buildingManager = ServiceLocator.Instance.GetService<BuildingManager>();

        interactableBehaviour.OnGetInteractablePositions.AddListener(GetInteractablePositions);
    }

    public void GetInteractablePositions(HashSet<Vector2Int> interactablePositions)
    {
        Vector2Int unitVectorInDirection = DirectionUtility.DirectionToVector(directionBehaviour.Direction);
        Vector2Int interactablePosition = buildingBehaviour.Position + unitVectorInDirection * range.x;
        for (int i = 0; i < range.y - range.x; i++)
        {
            if (!buildingManager.BuildingGrid.WithinGrid(interactablePosition))
                return;
            if (!buildingManager.HasBuilding(interactablePosition))
                interactablePositions.Add(interactablePosition);
            interactablePosition += unitVectorInDirection;
        }
    }
}
