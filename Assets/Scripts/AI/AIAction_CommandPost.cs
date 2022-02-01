using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAction_CommandPost : AIAction
{
    [Header("References")]
    [SerializeField] UnitSpawner unitSpawner = null;
    [SerializeField] SquadSpawner squadSpawner = null;
    [SerializeField] List<CommandPost> commandPost = new List<CommandPost>();
    public override void SelectAction()
    {
        List<CommandPost> workingList = new List<CommandPost>();

        CommandPost closest = GetClosestCommandPost(workingList);

        if (!closest)
            return;

        var position = closest.transform.position + new Vector3(10.0f,0.0f,0.0f);
        foreach (var u in unitSpawner.units)
        {
            if (u == null)
                continue;
            u.SetDestination(position);
        }

        foreach (var s in squadSpawner.squads)
        {
            if (s == null)
                continue;
            s.SetDestination(position);
        }
        base.SelectAction();
    }

    CommandPost GetClosestCommandPost(List<CommandPost> workingList)
    {
        GetLiveCommandPosts(workingList);
        float mag = 100000.0f;
        CommandPost closest = null;
        foreach (var c in workingList)
        {
            float nMag = (transform.position - c.transform.position).magnitude;
            if (nMag < mag)
            {
                closest = c;
                mag = nMag;
            }
        }
        return closest;
    }

    void GetLiveCommandPosts(List<CommandPost> workingList)
    {
        foreach (var c in commandPost)
        {
            var f = c.GetComponent<Faction>();
            if (f.FactionType == FactionType.Enemy)
                continue;
            workingList.Add(c);
        }
    }
}
