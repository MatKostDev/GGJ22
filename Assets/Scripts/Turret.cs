using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField]
    Transform barrelTransform;

    [SerializeField]
    TurretDetection detection;

    void Update()
    {
        Transform closestEnemy = null;

        float closestSqrDistance = Mathf.Infinity;

        for (int i = 0; i < detection.CurrentTargets.Count; i++)
        {
            if (!detection.CurrentTargets[i])
            {
                detection.CurrentTargets.RemoveAt(i);
                i--;
                continue;
            }

            float sqrDistance = (transform.position - detection.CurrentTargets[i].position).sqrMagnitude;

            if (sqrDistance < closestSqrDistance)
            {
                closestEnemy       = detection.CurrentTargets[i];
                closestSqrDistance = sqrDistance;
            }
        }

        if (closestEnemy)
        {
            barrelTransform.LookAt(closestEnemy);
        }
    }
}
