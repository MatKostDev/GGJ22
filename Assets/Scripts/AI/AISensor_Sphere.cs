using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISensor_Sphere : AISensor
{
    [SerializeField] List<string> focusTags = new List<string>();

    GameObject _sensedObject = null;

    private void Update()
    {
        if (_sensedObject != null && !_sensedObject.activeInHierarchy)
            _sensedObject = null;
    }
    public override bool Sense()
    {
        if (_sensedObject)
            return SetSenseTime();
        return false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (!focusTags.Contains(other.tag))
            return;

        if (_sensedObject == null || (transform.position - _sensedObject.transform.position).magnitude > (transform.position - other.transform.position).magnitude)
        {
            _sensedObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!focusTags.Contains(other.tag))
            return;

        if (_sensedObject == other.gameObject)
            _sensedObject = null;
    }

    public GameObject GetSensedObject()
    {
        return _sensedObject;
    }
}
