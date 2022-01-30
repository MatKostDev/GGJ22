using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDetection : MonoBehaviour
{
    List<Transform> m_currentTargets = new List<Transform>();

    int m_enemyLayer;

    public List<Transform> CurrentTargets
    {
        get => m_currentTargets;
    }

    void Awake()
    {
        m_enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    void OnTriggerEnter(Collider a_other)
    {
        if (a_other.gameObject.layer != m_enemyLayer || m_currentTargets.Contains(a_other.transform))
        {
            return;
        }

        m_currentTargets.Add(a_other.transform);
    }

    void OnTriggerExit(Collider a_other)
    {
        if (a_other.gameObject.layer != m_enemyLayer)
        {
            return;
        }

        m_currentTargets.Remove(a_other.transform);
    }
}
