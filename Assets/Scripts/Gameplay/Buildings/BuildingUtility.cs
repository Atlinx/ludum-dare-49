using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUtility
{
    public static Vector2Int GetGridFromWorld(Vector2 worldPos, int unitSize = 1)
    {

        int x = Mathf.RoundToInt(worldPos.x / unitSize);
        int y = Mathf.RoundToInt(worldPos.y / unitSize);

        return new Vector2Int(x, y);
    }

    public static Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x, gridPos.y, 0);
    }
}
