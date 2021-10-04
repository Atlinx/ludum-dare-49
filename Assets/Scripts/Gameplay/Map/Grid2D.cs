using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid2D<T>
{
    private T[][] grid;
    public Vector2Int Size { get; private set; }

    /// <summary>
    /// Create a x by y grid.
    /// </summary>
    /// <param name="sizeX"></param>
    /// <param name="sizeY"></param>
    public Grid2D(int sizeX, int sizeY) : this(new Vector2Int(sizeX, sizeY)) { }

    /// <summary>
    /// Creates a size.x by size.y grid.
    /// </summary>
    /// <param name="size"></param>
    public Grid2D(Vector2Int size)
    {
        this.Size = size;

        grid = new T[Size.x][];

        for (int i = 0; i < Size.x; i++)
        {
            grid[i] = new T[Size.y];
        }
    }


    /// <summary>
    /// Get the object from pos.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns>Stored data for that position</returns>
    public T Get(Vector2Int pos)
    {
        return Get(pos.x, pos.y);
    }

    /// <summary>
    /// Get the object from pos.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public T Get(int x, int y)
    {
        return grid[x][y];
    }

    
    /// <summary>
    /// Set the object of the grid position
    /// </summary>
    /// <param name="position"></param>
    /// <param name="obj"></param>
    public void Set(Vector2Int position, T obj)
    {
        Set(position.x, position.y, obj);
    }
    
    /// <summary>
    /// Set the object of the grid position
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="obj"></param>
    public void Set(int x, int y, T obj)
    {
        grid[x][y] = obj;
    }

    /// <summary>
    /// Removes object at pos and returns the removed object.
    /// <returns></returns>
    public T Remove(Vector2Int pos)
    {
        T original = grid[pos.x][pos.y];
        grid[pos.x][pos.y] = default(T);
        return original;
    }

    /// <summary>
    /// Swap objects of two coords
    /// </summary>
    /// <param name="pos1"></param>
    /// <param name="pos2"></param>
    public void Swap(Vector2Int pos1, Vector2Int pos2)
    {
        Swap(pos1.x, pos1.y, pos2.x, pos2.y);
    }
    
    /// <summary>
    /// Swap objects of two coords
    /// </summary>
    /// <param name="x1"></param>
    /// <param name="y1"></param>
    /// <param name="x2"></param>
    /// <param name="y2"></param>
    public void Swap(int x1, int y1, int x2, int y2)
    {
        var pos1Obj = grid[x1][y1];
        grid[x1][y1] = grid[x2][y2];
        grid[x2][y2] = pos1Obj;
    }

    public override string ToString()
    {

        string mapStr = "";
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                mapStr += "(" + grid[x][y] + ")";
            }

            if (x + 1 != Size.x)
            {
                mapStr += "\n";
            }
        }

        return mapStr;
    }

    /// <summary>
    /// Checks if a position is in the grid.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool WithinGrid(int x, int y)
    {
        return WithinGrid(new Vector2Int(x, y));
    }

    /// <summary>
    /// Checks if a position is in the grid.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool WithinGrid(Vector2Int position)
    {
        return position.x >= 0 && position.x < Size.x && position.y >= 0 && position.y < Size.y;
    }

    public Grid2D<T> ShallowCopy()
    {
        Grid2D<T> copy = new Grid2D<T>(Size.x, Size.y);
        for (int x = 0; x < Size.x; x++)
            for (int y = 0; y < Size.y; y++)
                copy.Set(x, y, Get(x, y));
        return copy;
    }

    public Grid2D<T> DeepCopy(Func<T, T> copyMethod)
    {
        Grid2D<T> copy = new Grid2D<T>(Size.x, Size.y);
        for (int x = 0; x < Size.x; x++)
            for (int y = 0; y < Size.y; y++)
                copy.Set(x, y, copyMethod.Invoke(Get(x, y)));
        return copy;
    }

    public Grid2D<U> Convert<U>(Func<T, U> convertMethod)
    {
        Grid2D<U> copy = new Grid2D<U>(Size.x, Size.y);
        for (int x = 0; x < Size.x; x++)
            for (int y = 0; y < Size.y; y++)
                copy.Set(x, y, convertMethod.Invoke(Get(x, y)));
        return copy;
    }
}
