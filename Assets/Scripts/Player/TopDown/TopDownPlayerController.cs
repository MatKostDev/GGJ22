using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;

public class TopDownPlayerController : PlayerControlType
{
    [SerializeField] UnityEvent SwappedTo = null;
    [SerializeField] UnityEvent SwappedFrom = null;
    [SerializeField] float speed = 0.5f;
    [Header("References")]
    [SerializeField] Animator unitPanelAnimator = null;
    [SerializeField] CinemachineVirtualCamera vCam = null;
    [SerializeField] Transform followObject = null;

    float _yOffset = 0.0f;
    private void Start()
    {
        _yOffset = followObject.position.y;
    }
    public override void OnUpdate()
    {
        Vector3 moveAxis = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));

        followObject.Translate(moveAxis*speed, Space.World);
        //vCam.transform.position = new Vector3(vCam.transform.position.x, _yOffset, vCam.transform.position.z);
        //followObject.transform.position = new Vector3(followObject.position.x, _yOffset, followObject.position.z);
    }

    public override void OnSwappedTo()
    {
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
