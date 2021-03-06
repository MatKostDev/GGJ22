using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour
{
    [System.Serializable]
    public class SquadUnit
    {
        public Unit unit;
        public Vector3 localStartPos;
    }
    public string squadName = "";
    public List<SquadUnit> squadUnits = new List<SquadUnit>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (var s in squadUnits)
            s.localStartPos = s.unit.transform.localPosition;
    }

    public void SetDestination(Vector3 dest, bool attackMove = true)
    {
        foreach (var s in squadUnits)
        {
            if (!s.unit || !s.unit.gameObject.activeSelf)
                continue;
            s.unit.SetDestination(dest,attackMove);
        }
    }

    private void Update()
    {
        if (IsDead())
            Destroy(gameObject);
    }

    public bool IsDead()
    {
        foreach (var s in squadUnits)
        {
            if (!s.unit)
                continue;
            if (s.unit.gameObject.activeSelf)
                return false;
        }
        return true;
    }
}
