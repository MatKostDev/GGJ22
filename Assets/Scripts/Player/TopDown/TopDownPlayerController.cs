using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class TopDownPlayerController : PlayerControlType
{
    [SerializeField] UnityEvent SwappedTo = null;
    [SerializeField] UnityEvent SwappedFrom = null;
    [SerializeField] UnityEvent OnSetUnitDestination = null;
    [SerializeField] float speed = 0.5f;
    [SerializeField] LayerMask layerMask = 0;

    [Header("References")]
    [SerializeField] Animator unitPanelAnimator = null;
    [SerializeField] CinemachineVirtualCamera vCam = null;
    [SerializeField] Transform followObject = null;
    [SerializeField] UnitPanel unitPanel = null;

    float _yOffset = 0.0f;
    Camera _mainCamera = null;
    private void Start()
    {
        _yOffset = followObject.position.y;
        _mainCamera = Camera.main;
    }
    public override void OnUpdate()
    {
        Vector3 moveAxis = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));

        if (moveAxis.sqrMagnitude > 1f)
        {
            moveAxis = moveAxis.normalized;
        }

        followObject.Translate(moveAxis * speed * Time.deltaTime, Space.World);
        MoveUnit();
    }

    void MoveUnit()
    {
        var unit = unitPanel.GetSelectedUnit();
        if (!Input.GetMouseButtonDown(1) || !unit || unit.CompareTag("EUnit") || !unit.CanMove())
            return;

        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        Vector3 position = Vector3.zero;

        if (Physics.Raycast(ray, out hitInfo, 100.0f, layerMask))
        {
            position = hitInfo.point;
            unit.SetDestination(position);
            OnSetUnitDestination.Invoke();
        }
    }

    public override void OnSwappedTo()
    {
        Vector3 newPosition = FirstPersonPlayerData.Instance.BodyTransform.position;
        newPosition.y = followObject.transform.position.y;

        followObject.transform.position = newPosition;

        vCam.Priority = 1;
        unitPanelAnimator.SetTrigger("Open");
        SwappedTo.Invoke();
    }

    public override void OnSwappedFrom()
    {
        vCam.Priority = 0;
        unitPanelAnimator.SetTrigger("Close");
        SwappedFrom.Invoke();
    }
}
