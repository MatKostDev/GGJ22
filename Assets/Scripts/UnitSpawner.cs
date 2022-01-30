using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] Unit unitPrefab = null;
    public UnityEvent OnSpawnUnit;
    [HideInInspector] public List<Unit> units = new List<Unit>();

    public void SpawnUnit()
    {
        OnSpawnUnit.Invoke();
        var g = GameObject.Instantiate(unitPrefab, transform.position, transform.rotation);
        units.Add(g);
    }

    private void Update()
    {
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i] == null)
            {
                units.RemoveAt(i);
                i--;
            }
        }
    }

}
