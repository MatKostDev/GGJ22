using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponDropManager : MonoBehaviour
{
    [SerializeField] List<GameObject> weaponDropPrefabs = new List<GameObject>();
    [SerializeField] UnityEvent OnSpawnWeapon = null;

    static WeaponDropManager _instance = null;
    // Start is called before the first frame update
    void Start()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            Debug.LogError("Multiple WeaponDropManagers are active! deactivating " + gameObject.name + "...");
            return;
        }

    }

    public static GameObject GetSpawnedWeapon()
    {
        _instance.OnSpawnWeapon.Invoke();
        return _instance.weaponDropPrefabs[Random.Range(0, _instance.weaponDropPrefabs.Count)];
    }
}
