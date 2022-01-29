using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEmit : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GetComponent<ResourceDropEmitter>().EmitLoot();

            Destroy(gameObject);
        }
    }
}
