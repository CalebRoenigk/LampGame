using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid
{
    public Dictionary<Vector2Int, Cell> Cells = new Dictionary<Vector2Int, Cell>();
    public Vector2Int LightDistanceRange = new Vector2Int(6, 10);
    private float _lampSpawnChance = 0f;

    public Grid()
    {
        
    }

    public Cell CreateCell(Vector2Int position, float delay = 0f)
    {
        if (!Cells.ContainsKey(position))
        {
            StructureType cellType = DetermineCellType(position);
            
            Cell newCell = new Cell(position);
            newCell.StructureType = cellType;
            Cells.Add(position, newCell);
            GridManager.Instance.GenerateCell(newCell, delay);
        }
        
        return Cells[position];
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

    public void CheckPlayerEffects(Vector2Int position)
    {
        if (Cells[position].GridObject is Battery)
        {
            ((Battery)Cells[position].GridObject).AddPowerToPlayer();
        }
    }

    public void SetStructure(StructureType structureType, Vector2Int position)
    {
        if (CheckExists(position))
        {
            Cells[position].StructureType = structureType;
        }
    }

    public StructureType DetermineCellType(Vector2Int position)
    {
        // Lamp spawning
        var nearbyLamps = Cells.Where(l =>
            l.Key.x >= position.x - LightDistanceRange.y && l.Key.x <= position.x + LightDistanceRange.y &&
            l.Key.y >= position.y - LightDistanceRange.y && l.Key.y <= position.y + LightDistanceRange.y &&
            l.Value.StructureType == StructureType.Lamp);

        bool canSpawnLamp = true;
        foreach (var lampCell in nearbyLamps)
        {
            if (Vector2Int.Distance(lampCell.Key, position) < LightDistanceRange.x)
            {
                canSpawnLamp = false;
                break;
            }
        }

        if (canSpawnLamp)
        {
            if (UnityEngine.Random.Range(0f, 1f) > 1f - _lampSpawnChance)
            {
                _lampSpawnChance = 0f;
                return StructureType.Lamp;
            }
            else
            {
                _lampSpawnChance += 0.1f;
            }
        }
        
        return StructureType.Empty;
    }
}
