using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAction_Move : AIAction
{
    [SerializeField] int sign = 1;
    [SerializeField] Vector2 movementRange = new Vector2(5.0f, 10.0f);

    [Header("References")]
    [SerializeField] NavMeshAgent agent = null;
    // Start is called before the first frame update
    public override void SelectAction()
    {
        NavMeshHit hit;
        var dist = AIBlackboard.RandomFloatHelper(movementRange);
        var destination = transform.position + AISensor.DirectionToPlayer(transform).normalized * sign * dist;
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!NavMesh.SamplePosition(destination, out hit, dist, NavMesh.AllAreas))
                destination = transform.position - AISensor.DirectionToPlayer(transform).normalized * sign * dist;
            agent.SetDestination(destination);
            agent.updateRotation = true;
        }
        base.SelectAction();
    }
}
