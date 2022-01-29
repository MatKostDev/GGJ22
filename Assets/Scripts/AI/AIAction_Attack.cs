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

    GameObject player = null;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerCapsule");
    }

    public override void SelectAction()
    {
        var dir = AISensor.DirectionToPlayer(transform);
        dir = Vector3.Lerp(dir, dir + player.GetComponent<Rigidbody>().velocity, AIBlackboard.RandomFloatHelper(aimLeading));

        var newRotation = Quaternion.LookRotation(dir.normalized);

        AIBlackboard.RotationHelper(weaponManager.transform.rotation, newRotation, weaponManager.transform, turnSpeedWhileStandingStill * 2.0f);

        if (weaponManager.CanPrimaryAttack())
            weaponManager.OnPrimaryWeapon();

        base.SelectAction();
    }

   
}
