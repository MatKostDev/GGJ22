using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] Animator animator = null;

    public void OnShoot()
    {
        animator.SetTrigger("Shoot");
    }

    public void OnMove()
    {
        animator.SetTrigger("Move");
    }

    public void OnDie()
    {
        animator.SetTrigger("Die");
    }

}
