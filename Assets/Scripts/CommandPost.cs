using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandPost : MonoBehaviour
{
    [SerializeField]
    Faction faction;

    int m_enemiesAdded;
    int m_alliesAdded;

    void Awake()
    {
        ChangeFaction(faction.FactionType);
    }

    void ChangeFaction(FactionType a_newFaction)
    {
        faction.FactionType = a_newFaction;
    }

    void OnTriggerEnter(Collider a_other)
    {
        if (!a_other.TryGetComponent<Faction>(out var faction))
        {
            return;
        }

        if (faction.FactionType == FactionType.Neutral)
        {
            
        }
    }
}
