using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandPost : MonoBehaviour
{
    [SerializeField]
    Faction faction;

    [SerializeField]
    int requiredUnitsForCapture = 5;

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
        if (!a_other.TryGetComponent<Faction>(out var otherFaction))
        {
            return;
        }

        if (faction.FactionType == FactionType.Neutral)
        {
            if (otherFaction.FactionType == FactionType.Enemy)
            {
                if (m_alliesAdded > 0)
                {
                    m_alliesAdded--;
                    OnAddedUnit(a_other.gameObject);
                }
                else
                {
                    m_enemiesAdded++;
                    OnAddedUnit(a_other.gameObject);
                }
            }
            else if (otherFaction.FactionType == FactionType.Friendly)
            {
                if (m_enemiesAdded > 0)
                {
                    m_enemiesAdded--;
                    OnAddedUnit(a_other.gameObject);
                }
                else
                {
                    m_alliesAdded++;
                    OnAddedUnit(a_other.gameObject);
                }
            }
        }
        else if (faction.FactionType == FactionType.Friendly)
        {
            if (otherFaction.FactionType == FactionType.Enemy)
            {
                m_enemiesAdded++;
                OnAddedUnit(a_other.gameObject);
            }
        }
        else if (faction.FactionType == FactionType.Enemy)
        {
            if (otherFaction.FactionType == FactionType.Friendly)
            {
                m_alliesAdded++;
                OnAddedUnit(a_other.gameObject);
            }
        }
    }

    void OnAddedUnit(GameObject a_unitObject)
    {
        Destroy(a_unitObject);

        if (m_alliesAdded >= requiredUnitsForCapture)
        {
            ChangeFaction(FactionType.Enemy);
        }
        else if (m_enemiesAdded >= requiredUnitsForCapture)
        {
            ChangeFaction(FactionType.Friendly);
        }
    }
}
