using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class TopDownPlayerController : PlayerControlType
{
    [SerializeField]
    CinemachineVirtualCamera vCam;

    public override void OnUpdate()
    {

    }

    public override void OnSwappedTo()
    {
        vCam.Priority = 1;
    }

    public override void OnSwappedFrom()
    {
        vCam.Priority = 0;
    }
}
