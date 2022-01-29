using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//using static UnityEngine.InputSystem.InputAction;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] float downtime = 0.25f;
    public UnityEvent OnChangeWeapon;
    public UnityEvent OnBeginAttack;
    public UnityEvent OnFinishAttack;

    [Header("References")]
    [SerializeField] Weapon primaryWeapon = null;
    [SerializeField] Weapon secondaryWeapon = null;
    [SerializeField] GameObject ownerEntity = null;

    [HideInInspector] public bool canAttack = true;

    float _internalDowntime = 0.0f;


    private void Awake()
    {
        primaryWeapon.weaponOwner = ownerEntity;
    }

    private void Update()
    {
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

    public void OnSecondaryWeapon()
    {
        if (!secondaryWeapon.gameObject.activeSelf || !secondaryWeapon.CanAttack())
            return;
        _internalDowntime = 0.0f;
        OnBeginAttack.Invoke();
        secondaryWeapon.Attack();
    }

    public void SetSecondaryWeapon(Weapon weapon)
    {
        secondaryWeapon = weapon;
    }

    public void ActivateSecondaryWeapon()
    {
        secondaryWeapon.gameObject.SetActive(true);
    }

    public bool CanPrimaryAttack()
    {
        return primaryWeapon.CanAttack();
    }
    public bool CanSecondaryAttack()
    {
        return secondaryWeapon.CanAttack();
    }

}
