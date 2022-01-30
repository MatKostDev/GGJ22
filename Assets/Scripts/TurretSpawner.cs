using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawner : Singleton<TurretSpawner>
{
    [SerializeField]
    Transform spawnTransform = null;

    [SerializeField]
    GameObject turretPrefab = null;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnTurret();
        }
    }

    public void SpawnTurret()
    {

    }
}
