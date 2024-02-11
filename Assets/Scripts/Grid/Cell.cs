using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Vector2Int Position;
    public GridObject GridObject;
    public bool IsTraversable;
    public bool IsCheckpoint;
    public bool IsPowered;
    public float Visibility;

    public Cell(Vector2Int position)
    {
        this.Position = position;
        this.IsTraversable = true;
        this.IsCheckpoint = false;
        this.IsPowered = false;
    }

    public void SetObject(GridObject gridObject)
    {
        this.GridObject = gridObject;
        this.IsTraversable = gridObject.IsTraversable;
        this.IsCheckpoint = gridObject.IsCheckpoint;
        gridObject.CellParent = this;
    }

    public void SetPower(bool power)
    {
        this.IsPowered = power;
        if (this.GridObject is PowerableObject)
        {
            ((PowerableObject)this.GridObject).IsPowered = power;
            ((PowerableObject)this.GridObject).SetState();
        }
    }

    public Vector3 GetWorldPosition()
    {
        return new Vector3(Position.x, 0f, Position.y);
    }

    public void SetVisibility(float visibility, float delay = 0f)
    {
        this.Visibility = visibility;
        this.GridObject.UpdateVisibility(delay);
    }
}
