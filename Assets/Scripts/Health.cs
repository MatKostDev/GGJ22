using System.Collections;
using System.Collections.Generic;
using Unity;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{

    [SerializeField] protected float health;

    public UnityEvent OnTakeDamage, OnDie;
    private bool deathDone = false;

    protected float _maxHP = 0.0f;

    private void Start()
    {
        _maxHP = health;
        OnDie.AddListener(Die);
    }

    public float GetHealth()
    {
        return health;
    }
    public float GetMaxHealth()
    {
        return _maxHP;
    }
    public float GetHealthPercentage()
    {
        return health / _maxHP;
    }

    public void SetToMaxHealth()
    {
        health = _maxHP;
    }
    public virtual void Die()
    {
    }
    public virtual void TakeDamage(float damage)
    {
        OnTakeDamage.Invoke();
        health -= damage;
        if (IsDead())
            OnDie.Invoke();
    }

    public bool IsDead()
    {
        return health <= 0.5f;
    }

}
