using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AISensor_Alert : AISensor
{
    [SerializeField] Vector2 staleSenseTimer = new Vector2(1.0f, 3.0f);
    public UnityEvent OnAlert;
    public UnityEvent OnStale;

    [Header("References")]
    [SerializeField] AISensor_Cone visionCone = null;
    [SerializeField] AISensor_Sphere sixthSenseSphere = null;

    bool _alerted = false;

    public override bool Sense()
    {

        if (visionCone.Sense() || sixthSenseSphere.Sense())
            return SetSenseTime();
        return false;
    }

    public bool Alerted()
    {
        Sense();
        float staleTime = AIBlackboard.RandomFloatHelper(staleSenseTimer);
        if (timeSinceLastSuccessfulSense < staleTime)
        {
            if (!_alerted)
            {
                _alerted = true;
                OnAlert.Invoke();
            }
            return true;
        }
        else
        {
            if (_alerted)
            {
                _alerted = false;
                OnStale.Invoke();
            }
            return false;
        }
    }
}
