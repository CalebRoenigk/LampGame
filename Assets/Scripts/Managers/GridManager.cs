using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    
    public Grid Grid = new Grid();
    public int StartGridSize = 5;
    
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
        for (int x = -(int)Mathf.Floor((float)StartGridSize/2f); x < (int)Mathf.Ceil((float)StartGridSize/2f); x++)
        {
            for (int y = -(int)Mathf.Floor((float)StartGridSize/2f); y < (int)Mathf.Ceil((float)StartGridSize/2f); y++)
            {
                Grid.CreateCell(new Vector2Int(x,y));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        foreach (KeyValuePair<Vector2Int, Cell> Cell in Grid.Cells)
        {
            if (Cell.Value.IsPowered)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.white;
            }
            Gizmos.DrawWireCube(new Vector3(Cell.Key.x, 0f, Cell.Key.y), new Vector3(1f, 0f, 1f));
        }
    }

    public Vector2Int FindStopInDirection(Vector2Int currentPosition, Vector2Int direction, int power, out int powerRemaining)
    {
        powerRemaining = power;
        // Check if the current position is held in the grid
        if (Grid.Cells.ContainsKey(currentPosition))
        {
            bool hitObstacle = false;
            Vector2Int checkPosition = currentPosition + direction;
            while (powerRemaining > 0 && !hitObstacle)
            {
                hitObstacle = Grid.CheckOccupied(checkPosition);

                if (!hitObstacle)
                {
                    checkPosition += direction;
                }

                powerRemaining--;
            }

            return checkPosition - direction;
        }

        return currentPosition;
    }
}
