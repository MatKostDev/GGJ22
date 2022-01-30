using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//holds info for other classes
public class Unit : MonoBehaviour
{
    public string unitName = "";
   [Header("References")]
   public Health health;
   public Gun gun;

}
