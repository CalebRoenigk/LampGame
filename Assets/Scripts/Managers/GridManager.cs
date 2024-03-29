using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    
    public Grid Grid = new Grid();
    public int StartGridSize = 5;
    public GameObject Wall;
    public GameObject Battery;
    public GameObject Lamp;
    public GameObject CellBase;
    public float MaxVisibilityDelay = 1f;
    public int Seed = 0;
    public float MaxSpawnChance = 0.9f;
    private float _lampSpawnChance = 0.9f;

    private List<Vector2Int> pts = new List<Vector2Int>();
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Random.InitState(Seed);
        float maxCentralDist = Mathf.Sqrt(Mathf.Pow(StartGridSize, 2) + Mathf.Pow(StartGridSize, 2));
        
        for (int x = -(int)Mathf.Floor((float)StartGridSize/2f); x < (int)Mathf.Ceil((float)StartGridSize/2f); x++)
        {
            for (int y = -(int)Mathf.Floor((float)StartGridSize/2f); y < (int)Mathf.Ceil((float)StartGridSize/2f); y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                float distFromOrigin = Vector2Int.Distance(Vector2Int.zero, position);
                float visibilityDelay = (distFromOrigin / maxCentralDist) * MaxVisibilityDelay;
                Grid.CreateCell(position, visibilityDelay);

                if (x == 2 && y == 0)
                {
                    GenerateLamp(Grid.Cells[position], visibilityDelay);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        foreach (KeyValuePair<Vector2Int, Cell> cell in Grid.Cells)
        {
            if (cell.Value.IsPowered)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.white;
            }

            Gizmos.DrawWireCube(new Vector3(cell.Key.x, 0f, cell.Key.y), new Vector3(1f, 0f, 1f));
        }
    }

    public Vector2Int FindStopInDirection(Vector2Int currentPosition, Vector2Int direction, int power, out int powerRemaining)
    {
        powerRemaining = power;
        // Check if the current position is held in the grid
        if (Grid.Cells.ContainsKey(currentPosition))
        {
            bool isTraversable = true;
            bool hitObstacle = false;
            bool isCheckpoint = false;
            Vector2Int checkPosition = currentPosition + direction;
            while (powerRemaining > 0 && !hitObstacle)
            {
                // Checks
                isTraversable = Grid.CheckTraversable(checkPosition);

                if (isTraversable)
                {
                    checkPosition += direction;
                    powerRemaining--;
                }
                
                // Checkpoint movement
                isCheckpoint = Grid.CheckCheckpoint(checkPosition);
                if (isCheckpoint && powerRemaining > 0)
                {
                    checkPosition += direction;
                    powerRemaining--;
                }
                
                // Stop check
                hitObstacle = !Grid.CheckExists(checkPosition) || isCheckpoint;
            }

            return checkPosition - direction;
        }

        return currentPosition;
    }
    
    // Lights up tiles, spawns new tiles if needed
    public void IlluminateTiles(Vector2Int center, int radius)
    {
        float maxCentralDist = Mathf.Sqrt(Mathf.Pow(radius, 2) + Mathf.Pow(radius, 2));
        
        for (int x = center.x - radius; x <= center.x + radius; x++)
        {
            for (int y = center.y - radius; y <= center.y + radius; y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                float distFromOrigin = Vector2Int.Distance(center, position);
                float visibilityDelay = (distFromOrigin / maxCentralDist) * MaxVisibilityDelay;
                    
                Grid.CreateCell(position, visibilityDelay);
            }
        }
    }
    
    // Generates a cell and any objects within it
    public void GenerateCell(Cell cell, float delay = 0f)
    {
        switch (cell.StructureType)
        {
            case StructureType.Empty:
            default:
                CellBase cellBase = Instantiate(CellBase, cell.GetWorldPosition(), Quaternion.identity, transform).GetComponent<CellBase>();
                cell.SetObject(cellBase);
                break;
            case StructureType.Lamp:
                Lamp lamp = Instantiate(Lamp, cell.GetWorldPosition(), Quaternion.identity, transform).GetComponent<Lamp>();
                cell.SetObject(lamp);
                break;
            case StructureType.Battery:
                Battery battery = Instantiate(Battery, cell.GetWorldPosition(), Quaternion.identity, transform).GetComponent<Battery>();
                cell.SetObject(battery);
                break;
            case StructureType.Wall:
                Wall wall = Instantiate(Wall, cell.GetWorldPosition(), Quaternion.identity, transform).GetComponent<Wall>();
                cell.SetObject(wall);
                break;
        }

        cell.SetVisibility(1f, delay);
    }

    private void GenerateLamp(Cell cell, float delay = 0f)
    {
        Lamp lamp = Instantiate(Lamp, cell.GetWorldPosition(), Quaternion.identity, transform).GetComponent<Lamp>();
        cell.SetObject(lamp);
        cell.SetVisibility(1f, delay);
        Grid.SetStructure(StructureType.Lamp, cell.Position);
    }
    
}
