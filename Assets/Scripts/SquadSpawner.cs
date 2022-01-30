using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SquadSpawner : MonoBehaviour
{
    [Tooltip("-1 for infinite")]
    [SerializeField] int maxNumSquads = 4;
    [SerializeField] Squad squadPrefab;
    public UnityEvent OnSpawnSquad;
    [HideInInspector] public List<Squad> squads = new List<Squad>();
    public void SpawnSquad()
    {
        if (squads.Count == maxNumSquads)
            return;
        OnSpawnSquad.Invoke();
        var g = GameObject.Instantiate(squadPrefab, transform.position, transform.rotation);
        squads.Add(g);
    }

    private void Update()
    {
        for (int i = 0; i < squads.Count; i++)
        {
            if (squads[i] == null)
            {
                squads.RemoveAt(i);
                i--;
            }
        }
    }
}
