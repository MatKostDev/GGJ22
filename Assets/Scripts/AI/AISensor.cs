using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISensor : MonoBehaviour
{
    [SerializeField] string sensorName = "";
    static protected GameObject player = null;
    static public Vector3 DirectionToPlayer(Transform transform) => player.transform.position - transform.position;
    static public float DistanceToPlayer(Transform transform) => DirectionToPlayer(transform).magnitude;

    protected float _senseTime = -1.0f;
    public float timeSinceLastSuccessfulSense => _senseTime == 1000.0f ? -1.0f : Time.time - _senseTime;

    int _instanceCounter = 0;
    private void Start()
    {
        _instanceCounter++;
        if (player == null)
            player = GameObject.Find("PlayerCapsule");
    }
    private void OnDestroy()
    {
        _instanceCounter--;
        if (_instanceCounter == 0)
            player = null;
    }
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
