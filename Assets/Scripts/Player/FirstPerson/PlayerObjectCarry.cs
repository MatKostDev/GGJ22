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

    CarriableObject m_carriedObject = null;

    //Vector3 m_initialOffset;

    public bool IsCarrying
    {
        get => m_carriedObject;
    }

    public void UpdateCarrying(Vector3 a_forwardDir, Vector3 a_position)
    {
        if (!m_carriedObject)
        {
            return;
        }

        Vector3 newPosition = a_position + (a_forwardDir * rayCheckDistance);// + m_initialOffset;

        m_carriedObject.DesiredPosition = newPosition;
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

        hitCarriable.OnHover();

        if (a_pickUpIfValid)
        {
            hitCarriable.MoveTarget.position += Vector3.up * CARRIED_HEIGHT_OFFSET;

            m_carriedObject = hitCarriable;
            hitCarriable.OnPickedUp();

            //Vector3 carriedObjectVector = m_carriedObject.transform.position - transform.position;

            //Vector3 extendedPosition =
            //    (rayCheckDistance - carriedObjectVector.magnitude) * carriedObjectVector.normalized +
            //    m_carriedObject.transform.position;

            //m_initialOffset = extendedPosition - (a_direction * rayCheckDistance + transform.position);
        }
    }

    public void TryDropCarriedObject()
    {
        if (!m_carriedObject || !m_carriedObject.CanBePlaced)
        {
            return;
        }

        m_carriedObject.GetComponent<CarriableObject>().OnPlacedDown();

        m_carriedObject = null;
    }
}
