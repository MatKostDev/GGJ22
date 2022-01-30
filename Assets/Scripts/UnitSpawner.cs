using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] Unit unitPrefab = null;
    public UnityEvent OnSpawnUnit;

    public void SpawnUnit()
    {
        OnSpawnUnit.Invoke();
        GameObject.Instantiate(unitPrefab, transform.position, transform.rotation);
    }

}
