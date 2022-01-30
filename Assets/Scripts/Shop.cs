using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField]
    TMP_Text turretsCostText;

    [SerializeField]
    TMP_Text unitsCostText;

    [SerializeField]
    int turretsCost;

    [SerializeField]
    int unitsCost;

    ResourceManager m_resourceManager;

    void Awake()
    {
        turretsCostText.text = turretsCost.ToString();
        unitsCostText.text   = unitsCost.ToString();

        m_resourceManager = ResourceManager.Instance;
    }

    public void TryBuyTurret()
    {
        if (m_resourceManager.CurrentAmount < turretsCost)
        {
            return;
        }

        m_resourceManager.SpendResources(turretsCost);
        TurretSpawner.Instance.SpawnTurret();
    }

    public void TryBuyUnits()
    {

    }
}
