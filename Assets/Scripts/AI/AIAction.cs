using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIAction : MonoBehaviour
{
    [SerializeField] string actionName = "";
    public UnityEvent OnActionSelected;
    public UnityEvent OnActionDeselected;

    float _selectedTime = -1.0f;
    public float timeSinceLastSelected => _selectedTime == 1000.0f ? -1.0f : Time.time - _selectedTime;

    public virtual void SelectAction()
    {
        _selectedTime = Time.time;
        OnActionSelected.Invoke();
    }
    public virtual void DeselectAction()
    {
        OnActionDeselected.Invoke();
    }
}
