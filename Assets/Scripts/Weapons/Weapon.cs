using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    [Header("References")]
    public GameObject weaponOwner = null;

    [Tooltip("-1 for infinite")]
    [SerializeField] protected float cooldown = 0.0f;

    public UnityEvent OnAttack;
    public UnityEvent OnEnableWeapon;
    public UnityEvent OnDisableWeapon;

    float _cooldown = 0.0f;
    protected bool _attacking = false;

    private void OnEnable()
    {
        _cooldown = cooldown;
        OnEnableWeapon.Invoke();
        IEnumerator Cooldown()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                _cooldown -= Time.deltaTime;
            }
        }
        StartCoroutine(Cooldown());
    }

    private void OnDisable()
    {
        OnDisableWeapon.Invoke();
    }

    protected float GetCooldown()
    {
        return _cooldown;
    }

    protected bool IsAttacking()
    {
        return _attacking;
    }

    public bool CanAttack()
    {
        return (!_attacking && _cooldown < 0.0f);
    }

    public virtual void Attack()
    {
        if (!CanAttack())
            return;
        _cooldown = cooldown;
        OnAttack.Invoke();
    }

}
