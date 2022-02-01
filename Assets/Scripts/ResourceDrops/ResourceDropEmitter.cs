using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDropEmitter : MonoBehaviour
{
    [SerializeField]
    ResourceDrop dropPrefab = null;

    [SerializeField]
    int numDropsToEmit = 10;

    public void EmitLoot(bool a_destroyAfterEmit = true)
    {
        for (int i = 0; i < numDropsToEmit; i++)
        {
            var newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

            newDrop.OnEmitted();
        }

        if (a_destroyAfterEmit)
        {
            Destroy(this);
        }
    }
}
