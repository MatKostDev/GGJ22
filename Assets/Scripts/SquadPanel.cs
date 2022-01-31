using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SquadPanel : MonoBehaviour
{
    [System.Serializable]
    public class SquadButton
    {
        [HideInInspector] public Squad squad;
        public Button button;
    }
    public UnityEvent OnSelectSquad;
    [Header("References")]
    [SerializeField] List<SquadButton> buttons = new List<SquadButton>();
    [SerializeField] SquadSpawner squadSpawner = null;

    Squad _selectedSquad = null;

    // Update is called once per frame
    void Update()
    {
        UpdateButtons();
    }

    void UpdateButtons()
    {
        foreach (var b in buttons)
            if (b.squad == null)
                b.button.gameObject.SetActive(false);
        int index = 0;
        foreach (var s in squadSpawner.squads)
        {
            buttons[index].button.gameObject.SetActive(true);
            buttons[index].squad = s;
            index++;
        }
    }

    public void SelectSquad(int index)
    {
        if (index < 0 || index >= squadSpawner.squads.Count)
            return;
        _selectedSquad = buttons[index].squad;
        OnSelectSquad.Invoke();
    }

    public Squad GetSelectedSquad()
    {
        return _selectedSquad;
    }
}
