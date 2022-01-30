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

    [Header("Mouse Sens")]
    [SerializeField]
    float mouseSens = 1f;

    [SerializeField]
    float minMouseSens = 0.1f;

    [SerializeField]
    float maxMouseSens = 20f;

    float m_currentFrameMouseX;
    float m_currentFrameMouseY;

    Transform         m_cameraTransform;
    PlayerObjectCarry m_carryObject;

    FirstPersonPlayerData m_playerData;

    CinemachineVirtualCamera m_vCam;

    Vector3 m_lastFramePosition;

    Vector3 m_spawnPosition;

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
        m_playerData = FirstPersonPlayerData.Instance;

        m_cameraTransform = m_playerData.PlayerCamera.transform;
        m_carryObject     = m_playerData.ObjectCarry;

        m_lastFramePosition = transform.position;

        m_spawnPosition = transform.position;
    }

    public override void OnUpdate()
    {
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

        m_carryObject.UpdateCarrying(m_cameraTransform.forward, m_cameraTransform.position);

        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            if (m_carryObject.IsCarrying)
            {
                m_carryObject.TryDropCarriedObject();
            }
            else
            {
                m_carryObject.PerformRayCheck(m_cameraTransform.position, m_cameraTransform.forward);
            }
        }
        else if (!m_carryObject.IsCarrying)
        {
            m_carryObject.PerformRayCheck(m_cameraTransform.position, m_cameraTransform.forward, false);
        }

        CheckDeathPlane();

        m_lastFramePosition = transform.position;
    }

    public override void OnSwappedTo()
    {
        m_playerData.VCamera.Priority = 1;

        m_lastFramePosition = transform.position;

        m_playerData.CanvasFpp.SetActive(true);
    }

    public override void OnSwappedFrom()
    {
        m_playerData.VCamera.Priority = 0;

        m_playerData.CanvasFpp.SetActive(false);
    }

    void CheckDeathPlane()
    {
        if (transform.position.y > deathPlaneHeight)
        {
            return;
        }

        var motor = FirstPersonPlayerData.Instance.Motor;

        motor.Teleport(m_spawnPosition);
    }

    public void UpdateMouseSens(float a_newSens)
    {
        mouseSens = a_newSens;
    }
}
