using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAction_Attack : AIAction
{
    [SerializeField] Vector2 aimLeading = Vector2.zero;//0.15
    [SerializeField] float turnSpeedWhileStandingStill = 2.5f;

    [Header("References")]
    [SerializeField] WeaponManager weaponManager = null;
    [SerializeField] NavMeshAgent agent = null;

    public override void SelectAction()
    {
        agent.SetDestination(transform.position);
        if (weaponManager.CanPrimaryAttack())
            weaponManager.OnPrimaryWeapon();

        base.SelectAction();
    }

   
}
