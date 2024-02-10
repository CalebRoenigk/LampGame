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
        Gizmos.color = Color.white;

        foreach (KeyValuePair<Vector2Int, Cell> Cell in Grid.Cells)
        {
            Gizmos.DrawWireCube(new Vector3(Cell.Key.x, 0f, Cell.Key.y), new Vector3(1f, 0f, 1f));
        }
    }

    public Vector2Int FindStopInDirection(Vector2Int currentPosition, Vector2Int direction)
    {
        // Check if the current position is held in the grid
        if (Grid.Cells.ContainsKey(currentPosition))
        {
            bool hitObstacle = false;
            Vector2Int checkPosition = currentPosition + direction;
            int iterations = 100;
            while (iterations >= 0 && !hitObstacle)
            {
                hitObstacle = Grid.CheckOccupied(checkPosition);

                if (!hitObstacle)
                {
                    checkPosition += direction;
                }

                iterations--;
            }

            return checkPosition - direction;
        }

        return currentPosition;
    }
}
