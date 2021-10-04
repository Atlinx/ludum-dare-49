using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class DirectionUtility
{
    public enum Enum
    {
        North,
        East,
        South,
        West,
    }

    public static Vector2Int NorthVector => new Vector2Int(0, 1);
    public static Vector2Int EastVector => new Vector2Int(1, 0);
    public static Vector2Int SouthVector => new Vector2Int(0, -1);
    public static Vector2Int WestVector => new Vector2Int(-1, 0);

    public static Vector2Int DirectionToVector(Enum directionEnum)
    {
        switch(directionEnum)
        {
            case Enum.North:
                return NorthVector;
            case Enum.East:
                return EastVector;
            case Enum.South:
                return SouthVector;
            case Enum.West:
                return WestVector;
        }

        return Vector2Int.zero;
    }

    public static Enum GetOppositeDirection(Enum directionEnum)
    {
        switch (directionEnum)
        {
            case Enum.North:
                return Enum.South;
            case Enum.East:
                return Enum.West;
            case Enum.South:
                return Enum.North;
            case Enum.West:
                return Enum.East;
        }
        return Enum.North;
    }
}
