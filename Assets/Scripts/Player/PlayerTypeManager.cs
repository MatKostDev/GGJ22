using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTypeManager : MonoBehaviour
{
    PlayerControlType m_activeType;

    FirstPersonPlayerController m_firstPersonController;
    TopDownPlayerController     m_topDownController;

    bool m_isPaused = false;

    void Awake()
    {
        m_firstPersonController = FirstPersonPlayerData.Instance.Controller;
        m_topDownController     = FindObjectOfType<TopDownPlayerController>();

        Application.targetFrameRate = 300;
    }

    void Start()
    {
        SwapToTopDown();
    }

    void Update()
    {
        if (m_isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                m_isPaused = false;
                LockCursor();
                Time.timeScale = 1f;
            }

            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_isPaused = true;
            UnlockCursor();
            Time.timeScale = 0f;
        }

        m_activeType.OnUpdate();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (m_activeType == m_firstPersonController)
            {
                SwapToTopDown();
            }
            else
            {
                SwapToFirstPerson();
            }
        }
    }

    void SwapToFirstPerson()
    {
        m_activeType = m_firstPersonController;
        LockCursor();

        m_topDownController.OnSwappedFrom();
        m_firstPersonController.OnSwappedTo();
    }

    void SwapToTopDown()
    {
        m_activeType = m_topDownController;
        UnlockCursor();

        m_firstPersonController.OnSwappedFrom();
        m_topDownController.OnSwappedTo();
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;
    }
}
