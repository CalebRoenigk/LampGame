using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerableObject : GridObject
{
    public bool IsPowered;
    
    public abstract void SetState();
}
