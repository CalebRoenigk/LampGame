using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : EmitterObject
{
    public bool EffectedPlayer = false;
    public int PowerAddition = 8;

    public void AddPowerToPlayer()
    {
        if (!EffectedPlayer)
        {
            PlayerController.Instance.Power += this.PowerAddition;
            EffectedPlayer = true;
        }
    }
}
