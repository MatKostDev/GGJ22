using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarriableObject : MonoBehaviour
{
    Collider m_collider;

    bool m_canBePlaced = false;

    public bool CanBePlaced
    {
        get => m_canBePlaced;
    }

    void Awake()
    {
        m_collider = GetComponent<Collider>();
    }

    public void OnPickedUp()
    {
        m_collider.isTrigger = true;

        m_canBePlaced = true;
    }

    public void OnPlacedDown()
    {
        m_collider.isTrigger = false;
    }

    void OnTriggerEnter(Collider a_other)
    {
        m_canBePlaced = false;
    }

    void OnTriggerStay(Collider a_other)
    {
        m_canBePlaced = false;
    }

    void OnTriggerExit(Collider a_other)
    {
        m_canBePlaced = true;
    }
}
