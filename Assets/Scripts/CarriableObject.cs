using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarriableObject : MonoBehaviour
{
    public UnityEvent onPickedUp   = new UnityEvent();
    public UnityEvent onPlacedDown = new UnityEvent();

    [SerializeField]
    Color validColor = Color.green;

    [SerializeField]
    Color invalidColor = Color.red;

    [SerializeField]
    float moveSpeed = 25f;

    [SerializeField]
    Transform boxcastPoint;

    [SerializeField]
    Vector3 boxcastSize;

    [SerializeField]
    Transform moveTarget;

    Collider m_collider;

    bool m_canBePlaced = false;
    bool m_isCarried   = false;

    Outline m_outline;

    bool m_showOutline = false;

    Vector3 m_desiredPosition;

    public Transform MoveTarget
    {
        get => moveTarget;
    }

    public Vector3 DesiredPosition
    {
        get => m_desiredPosition;
        set => m_desiredPosition = value;
    }

    public bool CanBePlaced
    {
        get => m_canBePlaced;
    }

    void Awake()
    {
        m_collider = GetComponent<Collider>();
        m_outline  = GetComponent<Outline>();

        m_outline.enabled = false;

        m_desiredPosition = moveTarget.position;
    }

    void Update()
    {
        moveTarget.position = Vector3.Lerp(moveTarget.position, m_desiredPosition, moveSpeed * Time.deltaTime);

        if (m_showOutline || m_isCarried)
        {
            m_outline.enabled = true;
        }
        else
        {
            m_outline.enabled = false;
        }

        if (!m_isCarried)
        {
            if (Physics.BoxCast(boxcastPoint.position, boxcastSize, Vector3.down, out var castHit, Quaternion.identity))
            {
                m_desiredPosition.y = castHit.point.y + boxcastSize.y;
            }
        }

        m_showOutline = false;
    }

    public void OnHover()
    {
        m_outline.OutlineColor = validColor;

        m_showOutline = true;
    }

    public void OnPickedUp()
    {
        m_collider.isTrigger = true;

        m_canBePlaced = true;

        m_outline.OutlineColor = validColor;

        m_isCarried = true;

        m_desiredPosition = moveTarget.position;

        onPickedUp?.Invoke();
    }

    public void OnPlacedDown()
    {
        m_collider.isTrigger = false;

        m_isCarried = false;

        m_desiredPosition = moveTarget.position;

        if (Physics.BoxCast(boxcastPoint.position, boxcastSize, Vector3.down, out var castHit, Quaternion.identity))
        {
            m_desiredPosition.y = castHit.point.y + boxcastSize.y;
        }

        onPlacedDown?.Invoke();
    }

    void OnTriggerEnter(Collider a_other)
    {
        if (a_other.isTrigger)
        {
            return;
        }

        m_canBePlaced = false;

        m_outline.OutlineColor = invalidColor;
    }

    void OnTriggerStay(Collider a_other)
    {
        if (a_other.isTrigger)
        {
            return;
        }

        m_canBePlaced = false;

        m_outline.OutlineColor = invalidColor;
    }

    void OnTriggerExit(Collider a_other)
    {
        if (a_other.isTrigger)
        {
            return;
        }

        m_canBePlaced = true;

        m_outline.OutlineColor = validColor;
    }
}
