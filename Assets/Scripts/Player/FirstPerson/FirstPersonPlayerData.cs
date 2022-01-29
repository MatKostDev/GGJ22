using Cinemachine;
using UnityEngine;

public class FirstPersonPlayerData : Singleton<FirstPersonPlayerData>
{
    [SerializeField]
    Transform bodyTransform;

    [SerializeField]
    PlayerMotor motor;

    [SerializeField]
    FirstPersonPlayerController controller;

    [SerializeField]
    PlayerRotationController rotationController;

    [SerializeField]
    CinemachineVirtualCamera vCamera;

    [SerializeField]
    Camera playerCamera;

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

    public FirstPersonPlayerController Controller
    {
        get => controller;
    }

    public PlayerRotationController RotationController
    {
        get => rotationController;
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
}
