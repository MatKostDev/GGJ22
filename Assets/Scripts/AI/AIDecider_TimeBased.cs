using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDecider_TimeBased : AIDecider
{
    [SerializeField] Vector2 sensingTimeRange = Vector2.zero;
    [SerializeField] Vector2 actionTimeRange = Vector2.zero;

    float _senseTime = 0.0f;
    float _actionTime = 0.0f;

    private void Start()
    {
        _senseTime = AIBlackboard.RandomFloatHelper(sensingTimeRange);
        _actionTime = AIBlackboard.RandomFloatHelper(actionTimeRange);
    }
    public override bool TryPerformActions()
    {
        bool performAction = true;
        foreach (var sensor in sensors)
        {
            performAction = sensor.Sense() && sensor.timeSinceLastSuccessfulSense > _senseTime;
            if (!performAction)
                return false;
        }
        _senseTime = AIBlackboard.RandomFloatHelper(sensingTimeRange);
        foreach (var action in acitons)
        {
            if (action.timeSinceLastSelected > _actionTime)
                action.SelectAction();

        }
        _actionTime = AIBlackboard.RandomFloatHelper(actionTimeRange);
        return true;
    }
}
