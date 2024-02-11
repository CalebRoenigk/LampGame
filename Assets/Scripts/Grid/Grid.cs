using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public Dictionary<Vector2Int, Cell> Cells = new Dictionary<Vector2Int, Cell>();

    public Grid()
    {
        
    }

    public void CreateCell(Vector2Int position, float delay = 0f)
    {
        if (!Cells.ContainsKey(position))
        {
            Cell newCell = new Cell(position);
            Cells.Add(position, newCell);
            GridManager.Instance.GenerateCell(newCell, delay);
        }
    }

    public bool CheckTraversable(Vector2Int position)
    {
        if (Cells.ContainsKey(position))
        {
            return Cells[position].IsTraversable;
        }
        
        return false;
    }

    public bool CheckCheckpoint(Vector2Int position)
    {
        if (Cells.ContainsKey(position))
        {
            return Cells[position].IsCheckpoint;
        }
        
        return false;
    }

    public bool CheckExists(Vector2Int position)
    {
        return Cells.ContainsKey(position);
    }

    public bool CheckCanMove(Vector2Int position)
    {
        if (!CheckExists(position))
        {
            return false;
        }

        return CheckTraversable(position);
    }

    public void SetPower(Vector2Int position, bool power)
    {
        if (Cells.ContainsKey(position))
        {
            Cells[position].SetPower(power);
        }
    }
}
