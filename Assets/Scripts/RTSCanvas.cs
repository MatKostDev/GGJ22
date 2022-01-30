using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSCanvas : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform trueParent = null;
    float _yOffset = 0;
    // Start is called before the first frame update
    void Start()
    {
        _yOffset = transform.position.y - trueParent.position.y;
        var worldSpaceCanvas = GameObject.Find("World Space");
        transform.SetParent(worldSpaceCanvas.transform);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = trueParent.transform.position + new Vector3(0.0f, _yOffset, 0.0f);
    }
}
