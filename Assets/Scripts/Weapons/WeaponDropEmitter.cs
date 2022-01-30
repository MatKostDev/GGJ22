using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponDropEmitter : MonoBehaviour
{
    [Range(0.0f,1.0f)]
    [SerializeField] float dropChance = 1.0f;

    public void TryDropWeapon(){
        if(dropChance == 0.0f)
            return;
        int chance = (int)(1.0f / dropChance);
        chance = Random.Range(1,101) % chance;
        Debug.Log(chance);
        if(chance != 0)
            return;
        GameObject w = WeaponDropManager.GetSpawnedWeapon();
        if(w == null)
            return;
        var nw = GameObject.Instantiate(w);
        nw.transform.position = transform.position;
    }
}
