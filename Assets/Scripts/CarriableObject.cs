using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarriableObject : MonoBehaviour
{
    [SerializeField]
    Color validColor = Color.green;

    [SerializeField]
    Color invalidColor = Color.red;

    Collider m_collider;

    bool m_canBePlaced = false;
    bool m_isCarried   = false;

    Outline m_outline;

    bool m_showOutline = false;

    public bool CanBePlaced
    {
        get => m_canBePlaced;
    }

    void Awake()
    {
        m_collider = GetComponent<Collider>();
        m_outline  = GetComponent<Outline>();

        m_outline.enabled = false;
    }

    void Update()
    {
        if (m_showOutline || m_isCarried)
        {
            m_outline.enabled = true;
        }
        else
        {
            m_outline.enabled = false;
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
    }

    public void OnPlacedDown()
    {
        m_collider.isTrigger = false;

        m_isCarried = false;
    }

    void OnTriggerEnter(Collider a_other)
    {
        m_canBePlaced = false;

        m_outline.OutlineColor = invalidColor;
    }

    void OnTriggerStay(Collider a_other)
    {
        m_canBePlaced = false;

        m_outline.OutlineColor = invalidColor;
    }

    void OnTriggerExit(Collider a_other)
    {
        m_canBePlaced = true;

        m_outline.OutlineColor = validColor;
    }
}
