using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CommandPost : MonoBehaviour
{
    public UnityEvent onAllyCapture  = new UnityEvent();
    public UnityEvent onEnemyCapture = new UnityEvent();

    [SerializeField]
    FactionType initialFaction;

    [SerializeField]
    bool isEndGoal = false;

    [SerializeField]
    int requiredUnitsForCapture = 5;

    [SerializeField]
    Color neutralColor = Color.gray;

    [SerializeField]
    Color friendlyColor = Color.green;

    [SerializeField]
    Color enemyColor = Color.red;

    int m_enemiesAdded;
    int m_alliesAdded;

    Faction m_faction;

    MeshRenderer m_meshRenderer;

    static int s_numControlledEnemy = 0;
    static int s_numControlledAlly  = 0;

    int m_totalNumCapturable;

    public bool IsEndGoal
    {
        get => isEndGoal;
    }

    void Awake()
    {
        m_faction      = GetComponent<Faction>();
        m_meshRenderer = GetComponent<MeshRenderer>();

        ChangeFaction(initialFaction, false);
    }

    void Start()
    {
        var posts = FindObjectsOfType<CommandPost>(true);
        foreach (var post in posts)
        {
            if (!post.isEndGoal)
            {
                m_totalNumCapturable++;
            }
        }
    }

    void ChangeFaction(FactionType a_newFaction, bool a_newEntry = true)
    {
        FactionType oldFaction = m_faction.FactionType;

        m_faction.FactionType = a_newFaction;

        Color newColor = neutralColor;

        if (a_newFaction == FactionType.Enemy)
        {
            newColor = enemyColor;
        }
        else if (a_newFaction == FactionType.Friendly)
        {
            newColor = friendlyColor;
        }

        m_meshRenderer.material.SetColor("_LineColor", newColor);

        if (!isEndGoal)
        {
            if (a_newFaction == FactionType.Friendly)
            {
                s_numControlledAlly++;

                if (oldFaction == FactionType.Enemy)
                {
                    s_numControlledEnemy--;
                }
            }
            else if (a_newFaction == FactionType.Enemy)
            {
                s_numControlledEnemy++;

                if (oldFaction == FactionType.Friendly)
                {
                    s_numControlledAlly--;
                }
            }
        }

        if (a_newEntry)
        {
            if (a_newFaction == FactionType.Friendly)
            {
                onAllyCapture?.Invoke();
            }
            else if (a_newFaction == FactionType.Enemy)
            {
                onEnemyCapture?.Invoke();
            }

            if (isEndGoal)
            {
                Invoke(nameof(LoadMainMenu), 1f);
            }
        }
    }

    void LoadMainMenu()
    {
	    Cursor.lockState = CursorLockMode.None;
	    Cursor.visible   = true;

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    void OnTriggerEnter(Collider a_other)
    {
        if (!a_other.TryGetComponent<Faction>(out var otherFaction))
        {
            return;
        }

        bool isOtherFaction = otherFaction.FactionType != m_faction.FactionType;
        if (!isOtherFaction)
        {
            return;
        }

        if (isEndGoal)
        {
            if ((otherFaction.FactionType == FactionType.Enemy && s_numControlledEnemy < m_totalNumCapturable)
                || (otherFaction.FactionType == FactionType.Friendly && s_numControlledAlly < m_totalNumCapturable))
            {
                return;
            }
        }

        if (m_faction.FactionType == FactionType.Neutral)
        {
            if (otherFaction.FactionType == FactionType.Enemy)
            {
                if (m_alliesAdded > 0)
                {
                    m_alliesAdded--;
                    OnAddedUnit(a_other.gameObject);
                }
                else
                {
                    m_enemiesAdded++;
                    OnAddedUnit(a_other.gameObject);
                }
            }
            else if (otherFaction.FactionType == FactionType.Friendly)
            {
                if (m_enemiesAdded > 0)
                {
                    m_enemiesAdded--;
                    OnAddedUnit(a_other.gameObject);
                }
                else
                {
                    m_alliesAdded++;
                    OnAddedUnit(a_other.gameObject);
                }
            }
        }
        else if (m_faction.FactionType == FactionType.Friendly)
        {
            if (otherFaction.FactionType == FactionType.Enemy)
            {
                m_enemiesAdded++;
                OnAddedUnit(a_other.gameObject);
            }
        }
        else if (m_faction.FactionType == FactionType.Enemy)
        {
            if (otherFaction.FactionType == FactionType.Friendly)
            {
                m_alliesAdded++;
                OnAddedUnit(a_other.gameObject);
            }
        }
    }

    void OnAddedUnit(GameObject a_unitObject)
    {
        Destroy(a_unitObject);

        if (m_alliesAdded >= requiredUnitsForCapture)
        {
            ChangeFaction(FactionType.Friendly);
        }
        else if (m_enemiesAdded >= requiredUnitsForCapture)
        {
            ChangeFaction(FactionType.Enemy);
        }
    }
}
