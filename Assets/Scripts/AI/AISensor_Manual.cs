using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISensor_Manual : AISensor
{
    public bool tripped = false;
    public void TripSensor()
    {
        tripped = true;
    }

    public override bool Sense()
    {
        if (tripped)
            return SetSenseTime();
        else
            return false;
    }

    public void ResetSensor()
    {
        tripped = false;
    }
}
