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
    [SerializeField] Transform enemyTransform = null;
    GameObject player = null;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerCapsule");
    }

    public override void SelectAction()
    {
        agent.updateRotation = false;
        var dir = AISensor.DirectionToPlayer(enemyTransform);
        dir = Vector3.Lerp(dir, dir + player.GetComponent<Rigidbody>().velocity, AIBlackboard.RandomFloatHelper(aimLeading));

        var newRotation = Quaternion.LookRotation(dir.normalized);

        AIBlackboard.RotationHelper(enemyTransform.rotation, newRotation, enemyTransform, turnSpeedWhileStandingStill);

        base.SelectAction();
    }
}
