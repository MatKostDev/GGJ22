using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//using static UnityEngine.InputSystem.InputAction;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] float downtime = 0.25f;
    public UnityEvent OnBeginAttack;
    public UnityEvent OnFinishAttack;

    [Header("References")]
    [SerializeField] Weapon primaryWeapon = null;
    [SerializeField] GameObject ownerEntity = null;

    [HideInInspector] public bool canAttack = true;

    float _internalDowntime = 0.0f;
    public bool _Debug = false;


    private void Awake()
    {
        primaryWeapon.weaponOwner = ownerEntity;
    }

    private void Update()
    {
        if (_Debug)
        {
            _Debug = false;
            OnPrimaryWeapon();
        }
        _internalDowntime += Time.deltaTime;
        if (_internalDowntime >= downtime)
            OnFinishAttack.Invoke();
    }

    public void OnPrimaryWeapon()
    {
        if (!primaryWeapon.CanAttack())
            return;
        _internalDowntime = 0.0f;
        OnBeginAttack.Invoke();
        primaryWeapon.Attack();
    }

    public bool CanPrimaryAttack()
    {
        return primaryWeapon.CanAttack();
    }
}
