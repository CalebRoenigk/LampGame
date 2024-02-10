using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Vector2Int Position;
    public bool IsOccupied;

    public Cell(Vector2Int position)
    {
        this.Position = position;
        this.IsOccupied = false;
    }
}