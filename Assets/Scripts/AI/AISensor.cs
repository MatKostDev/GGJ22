using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISensor : MonoBehaviour
{
    [SerializeField] string sensorName = "";

    protected float _senseTime = -1.0f;
    public float timeSinceLastSuccessfulSense => _senseTime == 1000.0f ? -1.0f : Time.time - _senseTime;

    public virtual bool Sense()
    {
        return false;
    }

    protected bool SetSenseTime()
    {
        _senseTime = Time.time;
        return true;
    }
}
