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

    public void RespawnExistingTurret(Transform a_moveTarget)
    {
        a_moveTarget.position = GetSpawnPosition();
        Debug.Log(a_moveTarget.position);
    }

    public void SpawnTurret()
    {
        Instantiate(turretPrefab, GetSpawnPosition(), spawnTransform.rotation);
    }

    Vector3 GetSpawnPosition()
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

        m_spawnNumber++;

        if (m_spawnNumber > 2)
        {
            m_spawnNumber = 1;

            m_multiplySign *= -1;
        }

        return spawnPosition;
    }
}
