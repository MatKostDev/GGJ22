using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerControlType : MonoBehaviour
{
    public abstract void OnUpdate();

    public abstract void OnSwappedTo();

    public abstract void OnSwappedFrom();
}
