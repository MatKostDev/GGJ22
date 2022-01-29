using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectCarry : MonoBehaviour
{
    [Header("Raycast Check")]
    [SerializeField]
    float rayCheckDistance = 2f;

    [SerializeField]
    LayerMask layersToCheck;

    const float CARRIED_HEIGHT_OFFSET = 0.1f;

    Transform m_carriedObject = null;

    public bool IsCarrying
    {
        get => m_carriedObject;
    }

    public void UpdateCarrying(Vector3 a_deltaPosition)
    {
        if (!m_carriedObject)
        {
            return;
        }

        m_carriedObject.position += a_deltaPosition;
    }

    public void PerformRayCheck(Vector3 a_position, Vector3 a_direction, bool a_pickUpIfValid = true)
    {
        if (!Physics.Raycast(a_position, a_direction, out var rayHit, rayCheckDistance, layersToCheck))
        {
            return;
        }

        if (!rayHit.transform.TryGetComponent<CarriableObject>(out var hitCarriable))
        {
            return;
        }

        if (a_pickUpIfValid)
        {
            rayHit.transform.position += Vector3.up * CARRIED_HEIGHT_OFFSET;

            m_carriedObject = rayHit.transform;
            hitCarriable.OnPickedUp();
        }
    }

    public void TryDropCarriedObject()
    {
        m_carriedObject.position -= Vector3.up * CARRIED_HEIGHT_OFFSET;

        m_carriedObject = null;
    }
}
