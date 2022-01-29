using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Death Plane / Respawn")]
    [SerializeField]
    [Tooltip("Just for sanity, but the player shouldn't be able to fall below the map")]
    float deathPlaneHeight = -50f;

    [SerializeField]
    float respawnHeight = 4f;

    [Header("Mouse Sens")]
    [SerializeField]
    float mouseSens = 1f;

    [SerializeField]
    float minMouseSens = 0.1f;

    [SerializeField]
    float maxMouseSens = 20f;

    float m_currentFrameMouseX;
    float m_currentFrameMouseY;

    Transform                m_cameraTransform;
    PlayerRotationController m_rotationController;

    PlayerData m_playerData;

    bool m_isPaused = false;

    bool m_canMove = true;

    public float DeathPlaneHeight
    {
        get => deathPlaneHeight;
        set => deathPlaneHeight = value;
    }

    public float CurrentFrameMouseX
    {
        get => m_currentFrameMouseX;
    }

    public float CurrentFrameMouseY
    {
        get => m_currentFrameMouseY;
    }

    public bool CanMove
    {
        get => m_canMove;
        set => m_canMove = value;
    }

    void Awake()
    {
        m_playerData = PlayerData.Instance;

        m_cameraTransform    = m_playerData.PlayerCamera.transform;
        m_rotationController = m_playerData.RotationController;

        LockCursor();
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

        bool jump = Input.GetButtonDown("Jump");

        //rotation
        m_currentFrameMouseX = Input.GetAxis("Mouse X");
        m_currentFrameMouseY = Input.GetAxis("Mouse Y");

        Vector2 lookAxis = new Vector2(m_currentFrameMouseX, m_currentFrameMouseY);
        lookAxis *= mouseSens;

        m_playerData.RotationController.UpdateRotations(lookAxis);

        //movement
        if (m_canMove)
        {
            Vector2 moveAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (moveAxis.sqrMagnitude > 1f)
            {
                moveAxis = moveAxis.normalized;
            }

            m_playerData.Motor.Move(moveAxis, jump);
        }

        CheckDeathPlane();
    }

    void CheckDeathPlane()
    {
        if (transform.position.y > deathPlaneHeight)
        {
            return;
        }

        var motor = PlayerData.Instance.Motor;

        Vector3 newPosition = motor.transform.position;
        newPosition.y = respawnHeight;

        motor.Teleport(newPosition);
    }

    public void UpdateMouseSens(float a_newSens)
    {
        mouseSens = a_newSens;
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
