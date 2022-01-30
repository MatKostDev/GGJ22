using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//holds info for other classes
public class Unit : MonoBehaviour
{
    public string unitName = "";
    [SerializeField] bool canMove = true;
    [Header("References")]
    public Health health;
    public Gun gun;
    [SerializeField] NavMeshAgent agent = null;

    public void SetDestination(Vector3 dest)
    {
        if (!canMove)
            return;
        agent.updateRotation = true;
        agent.SetDestination(dest);
    }

    public bool CanMove()
    {
        return canMove;
    }

}
