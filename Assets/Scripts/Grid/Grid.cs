using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid
{
    public Dictionary<Vector2Int, Cell> Cells = new Dictionary<Vector2Int, Cell>();
    public Vector2Int LightDistanceRange = new Vector2Int(6, 10);
    public Vector2Int BatteryDistanceRange = new Vector2Int(12, 16);
    public Vector2Int WallDistanceRange = new Vector2Int(9, 13);
    public Vector2Int WallLengthRange = new Vector2Int(3, 7);
    private float _lampSpawnChance = 0f;
    private float _batterySpawnChance = 0f;

    public Grid()
    {
        
    }

    public Cell CreateCell(Vector2Int position, float delay = 0f)
    {
        if (!Cells.ContainsKey(position))
        {
            StructureType cellType = DetermineCellType(position);

            // TODO: Wall Spawning should happen after a lamp is picked, in an area far outside of the visible range Wall center point, from there a mini for loop will flag a series of tiles (from a random wander) for walls
            if (cellType == StructureType.Lamp)
            {
                // Find a wall spawn location
                
            }
            
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
        
        // Battery Spawning
        var nearbyBatteries = Cells.Where(l =>
            l.Key.x >= position.x - BatteryDistanceRange.y && l.Key.x <= position.x + BatteryDistanceRange.y &&
            l.Key.y >= position.y - BatteryDistanceRange.y && l.Key.y <= position.y + BatteryDistanceRange.y &&
            l.Value.StructureType == StructureType.Battery);

        bool canSpawnBattery = true;
        foreach (var batteryCell in nearbyBatteries)
        {
            if (Vector2Int.Distance(batteryCell.Key, position) < BatteryDistanceRange.x)
            {
                canSpawnBattery = false;
                break;
            }
        }

        if (canSpawnBattery)
        {
            if (UnityEngine.Random.Range(0f, 1f) > 1f - _batterySpawnChance)
            {
                _batterySpawnChance = 0f;
                return StructureType.Battery;
            }
            else
            {
                _batterySpawnChance += 0.0625f;
            }
        }
        
        return StructureType.Empty;
    }
}
