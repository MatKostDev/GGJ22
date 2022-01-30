using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField]
    Transform barrelTransform;

    [SerializeField]
    TurretDetection detection;

    [SerializeField]
    WeaponManager weaponManager;

    [SerializeField]
    LayerMask rayCheckLayers;

    void Update()
    {
        Transform closestEnemy = null;

        float closestSqrDistance = Mathf.Infinity;

        for (int i = 0; i < detection.CurrentTargets.Count; i++)
        {
            var currentTarget = detection.CurrentTargets[i];

            if (!currentTarget)
            {
                detection.CurrentTargets.RemoveAt(i);
                i--;
                continue;
            }

            Vector3 enemyDirection = currentTarget.position - barrelTransform.position;
            if (!Physics.Raycast(barrelTransform.position, enemyDirection, out var rayHit, 999f, rayCheckLayers.value)
                || rayHit.transform != currentTarget)
            {
                continue;
            }

            float sqrDistance = (transform.position - currentTarget.position).sqrMagnitude;

            if (sqrDistance < closestSqrDistance)
            {
                closestEnemy       = currentTarget;
                closestSqrDistance = sqrDistance;
            }
        }

        if (closestEnemy)
        {
            barrelTransform.LookAt(closestEnemy);

            weaponManager.OnPrimaryWeapon();
        }
    }
}
