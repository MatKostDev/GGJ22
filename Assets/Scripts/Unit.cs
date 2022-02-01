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
    [SerializeField] AISensor_Manual manualSensor = null;
    [SerializeField] AIAction_Move move = null;
    [SerializeField] AIDecider moveDecider = null;
    [SerializeField] AIDecider attackDecider = null;
    public void SetDestination(Vector3 dest, bool attackMove = true)
    {
        Debug.Log(attackMove);
        if (!canMove)
            return;
        attackDecider.overrideAllActions = attackMove;
        moveDecider.overrideAllActions = !attackMove;
        move.destination = dest;
        manualSensor.TripSensor();
    }


    public bool CanMove()
    {
        return canMove;
    }

    public void OnDie()
    {
        IEnumerator Wait()
        {
            yield return new WaitForSeconds(0.75f);
            Destroy(gameObject);
        }
        StartCoroutine(Wait());
    }

}
