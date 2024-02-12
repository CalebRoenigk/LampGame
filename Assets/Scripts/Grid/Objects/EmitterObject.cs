using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterObject : GridObject
{
    public bool IsPowered = true;

    public void Setup()
    {
        ObjectMaterial.SetInt("_Powered", 1);
    }
}
