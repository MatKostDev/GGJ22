using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIDecider : MonoBehaviour
{
    public bool overrideAllActions = false;
    [SerializeField] string decisionName = "";
    [SerializeField] protected List<AISensor> sensors = new List<AISensor>();
    [SerializeField] protected List<AIAction> acitons = new List<AIAction>();

    public virtual bool TryPerformActions()
    {
        bool performAction = false;
        foreach (var sensor in sensors)
        {
            performAction = sensor.Sense();
            if (performAction)
                break;
        }
        if (!performAction)
            return false;
        foreach (var action in acitons)
            action.SelectAction();
        return true;
    }
}
