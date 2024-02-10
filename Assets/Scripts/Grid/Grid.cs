using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public Dictionary<Vector2Int, Cell> Cells = new Dictionary<Vector2Int, Cell>();

    public Grid()
    {
        
    }

    public void CreateCell(Vector2Int position)
    {
        Cell newCell = new Cell(position);
        
        Cells.Add(position, newCell);
    }

    public bool CheckOccupied(Vector2Int position)
    {
        if (Cells.ContainsKey(position))
        {
            return Cells[position].IsOccupied;
        }
        
        return true;
    }
}
