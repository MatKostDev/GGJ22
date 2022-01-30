using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDecider_NoSensor : AIDecider
{
    public override bool TryPerformActions()
    {
        foreach (var action in acitons)
            action.SelectAction();
        return true;
    }
}
