using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DirectionBehaviour : MonoBehaviour
{
    public DirectionUtility.Enum Direction => direction;
    [SerializeField]
    private DirectionUtility.Enum direction;

    public Vector2Int DirectionVector => DirectionUtility.DirectionToVector(Direction);
    public DirectionUtility.Enum OppositeDirection => DirectionUtility.GetOppositeDirection(Direction);
    public Vector2Int OppositeDirectionVector => DirectionUtility.DirectionToVector(OppositeDirection);
}