using System.Collections.Generic;
using UnityEngine;

public class SokobanGrid
{
    /// <summary>
    /// Underlying grid that holds all the blocks.
    /// </summary>
    public Grid2D<Block> Grid { get; set; }

    /// <summary>
    /// Keeps track of the move actions made on the grid.
    /// </summary>
    public Queue<MoveAction> MoveActionHistory { get; set; }

    /// <summary>
    /// Creates a grid using a Grid2D of Blocks.
    /// </summary>
    /// <param name="grid"></param>
    public SokobanGrid(Grid2D<Block> grid)
    {
        Grid = grid;
    }

    /// <summary>
    /// Creates a size.x by size.y grid.
    /// </summary>
    /// <param name="size"></param>
    public SokobanGrid(Vector2Int size)
    {
        Grid = new Grid2D<Block>(size);
    }

    /// <summary>
    /// Creates a sizeX by sizeY grid.
    /// </summary>
    /// <param name="sizeX"></param>
    /// <param name="sizeY"></param>
    public SokobanGrid(int sizeX, int sizeY) : this(new Vector2Int(sizeX, sizeY)) { }

    #region Basic Grid Operations
    /// <summary>
    /// Sets a block to a position.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="block"></param>
    public void Set(Vector2Int position, Block block)
    {
        Grid.Set(position, block);
    }

    /// <summary>
    /// Sets a block to a position.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="block"></param>
    public void Set(int x, int y, Block block)
    {
        Grid.Set(x, y, block);
    }

    /// <summary>
    /// Gets a block at a postion.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Block Get(Vector2Int position)
    {
        return Grid.Get(position);
    }

    /// <summary>
    /// Gets a block at a postion.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Block Get(int x, int y)
    {
        return Grid.Get(x, y);
    }

    /// <summary>
    /// Checks if a block exists at a position.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool Has(Vector2Int position)
    {
        return Get(position) != null;
    }

    /// <summary>
    /// Moves a building from startPosition to endPosition if endPosition is not occupied.
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="endPosition"></param>
    /// <returns></returns>
    public bool Move(Vector2Int startPosition, Vector2Int endPosition)
    {
        return Move(new MoveAction(startPosition, endPosition));
    }

    /// <summary>
    /// Moves a building using a MoveAction.
    /// </summary>
    /// <param name="moveAction"></param>
    /// <returns></returns>
    public bool Move(MoveAction moveAction)
    {
        if (!CanMove(moveAction.StartPosition, moveAction.EndPosition))
            return false;

        Grid.Set(moveAction.EndPosition, Grid.Remove(moveAction.StartPosition));
        MoveActionHistory.Enqueue(moveAction);
        return true;
    }

    /// <summary>
    /// Checks if a move is valid, which means it's in bounds and not occupied by a building.
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="endPosition"></param>
    /// <returns></returns>
    public bool CanMove(Vector2Int startPosition, Vector2Int endPosition)
    {
        if (!(WithinGrid(startPosition) || WithinGrid(endPosition)))
            return false;

        if (Grid.Get(startPosition) == null || Grid.Get(endPosition) != null)
            return false;

        return true;
    }
    #endregion

    #region Pushing
    public PushResult Push(Vector2Int position, Vector2Int direction, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var moveActionQueue = new Queue<MoveAction>();
            var resultType = PushOnce(position, direction, new HashSet<Vector2Int>(), moveActionQueue);
            if (resultType != PushResultType.Successful)
                return new PushResult(resultType, amount - i);
            while (moveActionQueue.Count > 0)
            {
                var moveAction = moveActionQueue.Dequeue();
                Move(moveAction.StartPosition, moveAction.EndPosition);
            }
        }
        return new PushResult(PushResultType.Successful);
    }

    public PushResult Push(Vector2Int position, DirectionUtility.Enum direction, int amount)
    {
        return Push(position, DirectionUtility.DirectionToVector(direction), amount);
    }

    private PushResultType PushOnce(Vector2Int position, Vector2Int direction, HashSet<Vector2Int> movedPositions, Queue<MoveAction> moveActionQueue)
    {
        var building = Grid.Get(position);
        
        if (movedPositions.Contains(position))
            return PushResultType.Successful;
        movedPositions.Add(position);

        if (building == null)
            return PushResultType.NoBuildingFound;
        if (!building.Movable)
            return PushResultType.ImmovableBuilding;
        if (building is Connector connector)
        {
            // We are a connector, so we must push all connected buildings.
            foreach (Vector2Int connectionPos in connector.Connections)
            {
                var resultType = PushOnce(connectionPos, direction, movedPositions, moveActionQueue);
                if (resultType != PushResultType.Successful)
                    return resultType;
            }
        }

        // Attempts to push the building in front of the current
        // position is there is any.
        var pushedIntoBuildingPos = position + direction;
        var pushedIntoBuilding = Grid.Get(pushedIntoBuildingPos);
        if (pushedIntoBuilding != null)
        {
            var resultType = PushOnce(position + direction, direction, movedPositions, moveActionQueue);
            if (resultType != PushResultType.Successful)
                return resultType;
        }

        // The building in front has been cleared -- either the
        // spot in front was empty or it has been pushed
        // successfully by our attempts before.
        moveActionQueue.Enqueue(new MoveAction(position, pushedIntoBuildingPos));
        return PushResultType.Successful;
    }
    #endregion
    
    #region Helper
    public bool WithinGrid(int x, int y)
    {
        return x < Grid.Size.x && y < Grid.Size.y && x >= 0 && y >= 0;
    }

    public bool WithinGrid(Vector2Int position)
    {
        return WithinGrid(position.x, position.y);
    }

    public SokobanGrid ShallowCopy()
    {
        return new SokobanGrid(Grid.ShallowCopy());
    }

    public SokobanGrid DeepCopy()
    {
        Grid2D<Block> copy = Grid.DeepCopy((tile) => {
            return tile?.DeepCopy();
        });

        return new SokobanGrid(copy);
    }
    #endregion

    #region Nested Classes
    public class Block
    {
        public Block(bool movable)
        {
            Movable = movable;
        }

        public bool Movable { get; set; }

        public virtual Block DeepCopy()
        {
            return new Block(Movable);
        }
    }

    public class Connector : Block
    {
        public HashSet<Vector2Int> Connections { get; set; }

        public Connector(bool movable, HashSet<Vector2Int> connections) :
            base(movable)
        {
            Connections = connections;
        }

        public override Block DeepCopy()
        {
            return new Connector(Movable, new HashSet<Vector2Int>(Connections));
        }
    }
    #endregion
}

#region Push Classes
public enum PushResultType
{
    Successful,
    NoBuildingFound,
    ImmovableBuilding,
}

public class PushResult
{
    public PushResult(PushResultType type)
    {
        Type = type;
        LeftOverSpaces = 0;
    }

    public PushResult(PushResultType type, int leftOverSpaces)
    {
        Type = type;
        LeftOverSpaces = leftOverSpaces;
    }

    public PushResultType Type { get; set; }
    public int LeftOverSpaces { get; set; }
}

public class MoveAction
{
    public Vector2Int StartPosition { get; set; }
    public Vector2Int EndPosition { get; set; }

    public MoveAction(Vector2Int startPosition, Vector2Int endPosition)
    {
        this.StartPosition = startPosition;
        this.EndPosition = endPosition;
    }
}
#endregion