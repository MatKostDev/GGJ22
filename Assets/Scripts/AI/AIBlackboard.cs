using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//As a note to all of this, while the implementation is modular,
//if the underlying circumstances of the design change, I will
//have to go back and refactor, but for a first attempt im quite
//proud
public class AIBlackboard : MonoBehaviour
{
    [SerializeField] Vector2 timeBetweenDecisions = Vector2.zero;

    [Header("References")]
    [SerializeField] AISensor_Alert alertSensor = null;
    [SerializeField] AIDecider_TimeBased patrol = null;
    [SerializeField] List<AIDecider> deciders = new List<AIDecider>();

    float _decisionTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        _decisionTimer = RandomFloatHelper(timeBetweenDecisions);
    }

    // Update is called once per frame
    void Update()
    {
        if (!alertSensor.Alerted())
        {
            patrol.TryPerformActions();
            return;
        }
        _decisionTimer -= Time.deltaTime;
        if (_decisionTimer <= 0.0f)
        {
            foreach (var decision in deciders)
            {
                if (decision.TryPerformActions())
                {
                    _decisionTimer = RandomFloatHelper(timeBetweenDecisions);
                    if (decision.overrideAllActions)
                        return;
                }
            }
        }
    }

    public static float RandomFloatHelper(Vector2 range)
    {
        return Random.Range(range.x, range.y);
    }

    public static void RotationHelper(Quaternion currentRotation, Quaternion newRotation, Transform t, float turnSpeed)
    {
        var quatAngle = Quaternion.Angle(currentRotation, newRotation);
        if (quatAngle > 2.0f)
        {
            var rot = Quaternion.Slerp(t.rotation, newRotation, Time.deltaTime * turnSpeed);
            t.rotation = rot;
        }
        else
            t.rotation = newRotation;
    }
}
