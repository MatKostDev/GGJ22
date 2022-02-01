using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAction_RotateTowardsPlayer : AIAction
{
    [SerializeField] Vector2 aimLeading = Vector2.zero;//0.15
    [SerializeField] float turnSpeedWhileStandingStill = 2.5f;

    [Header("References")]
    [SerializeField] NavMeshAgent agent = null;
    [SerializeField] Transform parentTransform = null;
    [SerializeField] AISensor_Sphere sphere = null;
    [SerializeField] Transform emitterPoint = null;

    public override void SelectAction()
    {
        var sensed = sphere.GetSensedObject();
        if (sensed == null)
            return;
        agent.updateRotation = false;
        var dir = sensed.transform.position - transform.position;
        dir = dir.normalized;
        dir = Vector3.Lerp(dir, dir + sphere.GetSensedObject().GetComponent<Rigidbody>().velocity, AIBlackboard.RandomFloatHelper(aimLeading));

        var newRotation = Quaternion.LookRotation(dir.normalized);

        AIBlackboard.RotationHelper(parentTransform.rotation, newRotation, parentTransform, turnSpeedWhileStandingStill);

        if (emitterPoint)
        {
            emitterPoint.LookAt(sensed.transform.position);
        }

        base.SelectAction();
    }
}
