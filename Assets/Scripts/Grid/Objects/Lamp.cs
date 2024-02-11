using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : PowerableObject
{
    public int LightRadius = 5;

    private void Start()
    {
        ObjectMaterial.SetVector("_Emit_Offset", new Vector2(0, -19));
    }

    public override void SetState()
    {
        if (IsPowered)
        {
            // Request new tiles from the grid manager
            GridManager.Instance.IlluminateTiles(CellParent.Position, LightRadius);
        }
        
        ObjectMaterial.SetInt("_Powered", IsPowered ? 1 : 0);
    }
}
