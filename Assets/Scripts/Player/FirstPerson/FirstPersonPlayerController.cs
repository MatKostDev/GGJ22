using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonPlayerController : PlayerControlType
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

    FirstPersonPlayerData m_firstPersonPlayerData;

    CinemachineVirtualCamera m_vCam;

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
        m_firstPersonPlayerData = FirstPersonPlayerData.Instance;

        m_cameraTransform    = m_firstPersonPlayerData.PlayerCamera.transform;
        m_rotationController = m_firstPersonPlayerData.RotationController;
    }

    public override void OnUpdate()
    {
        bool jump = Input.GetButtonDown("Jump");

        //rotation
        m_currentFrameMouseX = Input.GetAxis("Mouse X");
        m_currentFrameMouseY = Input.GetAxis("Mouse Y");

        Vector2 lookAxis = new Vector2(m_currentFrameMouseX, m_currentFrameMouseY);
        lookAxis *= mouseSens;

        m_firstPersonPlayerData.RotationController.UpdateRotations(lookAxis);

        //movement
        if (m_canMove)
        {
            Vector2 moveAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (moveAxis.sqrMagnitude > 1f)
            {
                moveAxis = moveAxis.normalized;
            }

            m_firstPersonPlayerData.Motor.Move(moveAxis, jump);
        }

        CheckDeathPlane();
    }

    public override void OnSwappedTo()
    {
        m_firstPersonPlayerData.VCamera.Priority = 1;
    }

    public override void OnSwappedFrom()
    {
        m_firstPersonPlayerData.VCamera.Priority = 0;
    }

    void CheckDeathPlane()
    {
        if (transform.position.y > deathPlaneHeight)
        {
            return;
        }

        var motor = FirstPersonPlayerData.Instance.Motor;

        Vector3 newPosition = motor.transform.position;
        newPosition.y = respawnHeight;

        motor.Teleport(newPosition);
    }

    public void UpdateMouseSens(float a_newSens)
    {
        mouseSens = a_newSens;
    }
}
