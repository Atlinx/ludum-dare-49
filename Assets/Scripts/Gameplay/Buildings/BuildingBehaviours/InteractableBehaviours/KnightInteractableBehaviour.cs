using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class KnightInteractableBehaviour: MonoBehaviour
{
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
        for (int x = -2; x <= 2; x++)
        {
            if (x == 0)
                continue;
            for (int y = -2; y <= 2; y++)
            {
                if (y == 0 || x - y == 0 || x + y == 0)
                    continue;
                var pos = new Vector2Int(x, y) + buildingBehaviour.Position;
                if (buildingManager.BuildingGrid.WithinGrid(pos) && !buildingManager.HasBuilding(pos))
                    positions.Add(pos);
            }
        }
    }
}