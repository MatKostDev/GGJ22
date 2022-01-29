using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISensor_Sphere : AISensor
{
    [SerializeField] Vector2 area = new Vector2(5.0f, 10.0f);
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, area.y);
        Gizmos.DrawWireSphere(transform.position, area.x);
    }
    public override bool Sense()
    {
        var dist = DistanceToPlayer(transform);
        if (dist <= area.y && dist >= area.x)
            return SetSenseTime();
        return false;
    }
}
