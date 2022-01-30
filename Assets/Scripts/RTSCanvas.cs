using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSCanvas : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Unit unit = null;
    float _yOffset = 0;
    // Start is called before the first frame update
    void Start()
    {
        _yOffset = transform.position.y - unit.transform.position.y;
        var worldSpaceCanvas = FindObjectOfType<TopDownPlayerCanvas>(true);
        transform.SetParent(worldSpaceCanvas.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (!unit)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = unit.transform.position + new Vector3(0.0f, _yOffset, 0.0f);
    }

    public void SelectUnit()
    {
        UnitPanel.SelectUnit(unit);
    }
}
