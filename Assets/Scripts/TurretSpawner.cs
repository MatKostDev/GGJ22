using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawner : Singleton<TurretSpawner>
{
    [SerializeField]
    Transform spawnTransform = null;

    [SerializeField]
    GameObject turretPrefab = null;

    int m_spawnNumber  = 1;
    int m_multiplySign = 1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnTurret();
        }
    }

    public void SpawnTurret()
    {
        Vector3 spawnPosition = spawnTransform.position;

        if (m_spawnNumber % 2 == 0)
        {
            spawnPosition.z += 0.1f * m_multiplySign;
        }
        else if (m_spawnNumber % 2 == 1)
        {
            spawnPosition.x += 0.1f * m_multiplySign;
        }

        Instantiate(turretPrefab, spawnPosition, spawnTransform.rotation);

        m_spawnNumber++;

        if (m_spawnNumber > 2)
        {
            m_spawnNumber = 1;

            m_multiplySign *= -1;
        }
    }
}
