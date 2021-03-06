using Cinemachine;
using UnityEngine;

public class FirstPersonPlayerData : Singleton<FirstPersonPlayerData>
{
    [SerializeField]
    Transform bodyTransform;

    [SerializeField]
    PlayerMotor motor;

    [SerializeField]
    GrappleHook grapple;

    [SerializeField]
    FirstPersonPlayerController controller;

    [SerializeField]
    PlayerRotationController rotationController;

    [SerializeField]
    PlayerObjectCarry objectCarry;

    [SerializeField]
    CinemachineVirtualCamera vCamera;

    [SerializeField]
    Camera playerCamera;

    [SerializeField]
    GameObject canvasFpp;

    public static int PlayerLayer
    {
        get => LayerMask.NameToLayer("Player");
    }

    public Transform BodyTransform
    {
        get => bodyTransform;
    }

    public PlayerMotor Motor
    {
        get => motor;
    }

    public GrappleHook Grapple
    {
        get => grapple;
    }

    public FirstPersonPlayerController Controller
    {
        get => controller;
    }

    public PlayerRotationController RotationController
    {
        get => rotationController;
    }

    public PlayerObjectCarry ObjectCarry
    {
        get => objectCarry;
    }

    public CinemachineVirtualCamera VCamera
    {
        get => vCamera;
    }

    public Camera PlayerCamera
    {
        get => playerCamera;
    }

    public float GravityStrength
    {
        get => motor.GravityStrength;
    }

    public GameObject CanvasFpp
    {
        get => canvasFpp;
    }
}
