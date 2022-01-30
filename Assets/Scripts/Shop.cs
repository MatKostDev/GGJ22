using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    Image turretCooldownImage;

    [SerializeField]
    Image unitCooldownImage;

    [SerializeField]
    float turretCooldown;

    [SerializeField]
    float unitCooldown;

    ResourceManager m_resourceManager;

    float m_turretTimer;
    float m_unitTimer;

    [Header("References")]
    [SerializeField] SquadSpawner squadSpawner = null;

    void Awake()
    {
        turretsCostText.text = turretsCost.ToString();
        unitsCostText.text = unitsCost.ToString();

        m_resourceManager = ResourceManager.Instance;
    }

    void Update()
    {
        m_turretTimer = Mathf.Max(0f, m_turretTimer - Time.deltaTime);
        m_unitTimer = Mathf.Max(0f, m_unitTimer - Time.deltaTime);

        turretCooldownImage.fillAmount = m_turretTimer / turretCooldown;
        unitCooldownImage.fillAmount = m_unitTimer / unitCooldown;
    }

    public void TryBuyTurret()
    {
        if (m_resourceManager.CurrentAmount < turretsCost || m_turretTimer > 0.001f)
        {
            return;
        }

        m_resourceManager.SpendResources(turretsCost);
        TurretSpawner.Instance.SpawnTurret();

        m_turretTimer = turretCooldown;

        turretCooldownImage.fillAmount = 1f;
    }

    public void TryBuyUnits()
    {
        if (m_resourceManager.CurrentAmount < turretsCost || m_unitTimer > 0.001f)
        {
            return;
        }

        m_resourceManager.SpendResources(unitsCost);
        squadSpawner.SpawnSquad();
        m_unitTimer = unitCooldown;

        unitCooldownImage.fillAmount = 1f;
    }
}
