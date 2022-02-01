using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAction_Move : AIAction
{

    public Vector3 destination = Vector3.zero;
    [Header("References")]
    [SerializeField] NavMeshAgent agent = null;
    [SerializeField] AISensor_Manual manualSensor = null;
    // Start is called before the first frame update

    //come back to this since movement will work different for units
    public override void SelectAction()
    {
        if ((transform.position - destination).magnitude <= 1.0f)
            manualSensor.ResetSensor();
        agent.updateRotation = true;
        agent.SetDestination(destination);

        base.SelectAction();
    }
}
