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
    [SerializeField] float scrollSpeed = 8.0f;
    [SerializeField] LayerMask layerMask = 0;
    [SerializeField] Vector2 scrollMinMax = new Vector2(20.0f, 69.0f);

    [Header("References")]
    [SerializeField] Animator unitPanelAnimator = null;
    [SerializeField] CinemachineVirtualCamera vCam = null;
    [SerializeField] Transform followObject = null;
    [SerializeField] UnitPanel unitPanel = null;
    [SerializeField] SquadPanel squadPanel = null;

    float _yOffset = 0.0f;
    Camera _mainCamera = null;

    bool unitOrSquad = false;
    private void Start()
    {
        //scrollMinMax.x = followObject.transform.localPosition.x;
        _yOffset = followObject.position.y;
        _mainCamera = Camera.main;
    }
    public override void OnUpdate()
    {
        Vector3 moveAxis = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));

        if (moveAxis.sqrMagnitude > 1f)
            moveAxis = moveAxis.normalized;

        moveAxis += new Vector3(0.0f, Zoom(), 0.0f);
        followObject.Translate(moveAxis * speed * Time.deltaTime, Space.World);

        Vector3 newPosition = followObject.position;
        newPosition.y = Mathf.Clamp(newPosition.y, scrollMinMax.x, scrollMinMax.y);

        followObject.position = newPosition;
        
        if (!unitOrSquad)
            MoveUnit();
        else
            MoveSquad();
    }

    float Zoom()
    {
        var y = Input.mouseScrollDelta.y;
        var oldY = followObject.transform.localPosition.y;
        if (Mathf.Abs(y) <= 0.3f)
            return 0.0f;
        float scrollValue = -y * scrollSpeed;
        oldY += scrollValue;
        if (oldY <= scrollMinMax.x || oldY >= scrollMinMax.y)
            return 0.0f;
        return scrollValue;
    }

    bool MoveUnit()
    {
        var unit = unitPanel.GetSelectedUnit();
        if (!Input.GetMouseButtonDown(1) || !unit || unit.CompareTag("EUnit") || !unit.CanMove())
            return false;

        var position = RaycastHelper();
        if (position.magnitude > 0.0f)
        {
            unit.SetDestination(position,!Input.GetKey(KeyCode.LeftShift));
            OnSetUnitDestination.Invoke();
        }
        return true;
    }

    bool MoveSquad()
    {
        var squad = squadPanel.GetSelectedSquad();
        if (!Input.GetMouseButtonDown(1) || !squad || squad.CompareTag("EUnit"))
            return false;
        var position = RaycastHelper();
        if (position.magnitude > 0.0f)
        {
            squad.SetDestination(position,!Input.GetKey(KeyCode.LeftShift));
            OnSetUnitDestination.Invoke();
        }
        return true;
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

    Vector3 RaycastHelper()
    {
        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        Vector3 position = Vector3.zero;
        Physics.Raycast(ray, out hitInfo, 1000.0f, layerMask);
        return hitInfo.point;
    }

    public void OnSelectUnit()
    {
        unitOrSquad = false;
    }

    public void OnSelectSquad()
    {
        unitOrSquad = true;
    }
}
