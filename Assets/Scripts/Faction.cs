using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FactionType
{
    Neutral,
    Friendly,
    Enemy,
}

public class Faction : MonoBehaviour
{
    [SerializeField]
    FactionType faction;

    public FactionType FactionType
    {
        get => faction;
        set => faction = value;
    }
}
