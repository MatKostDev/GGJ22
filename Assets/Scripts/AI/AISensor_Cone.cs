using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISensor_Cone : AISensor
{
    [SerializeField] float sightConeAngle;
    [SerializeField] float sightConeLength;

    [Header("References")]
    [SerializeField] Transform enemyTransform = null;
    float _angle = 0.0f;

    // Update is called once per frame
    void Update()
    {
        _angle = Vector3.Angle(enemyTransform.forward, DirectionToPlayer(enemyTransform).normalized);
    }
    public override bool Sense()
    {
        if (Mathf.Abs(_angle) <= sightConeAngle && DistanceToPlayer(enemyTransform) <= sightConeLength)
            return SetSenseTime();
        return false;
    }
}
