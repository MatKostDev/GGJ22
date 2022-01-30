using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UnitPanel : MonoBehaviour
{
    [System.Serializable]
    public class StatBar
    {
        public Slider bar;
        public TMPro.TextMeshProUGUI number;

        public void SetActive(bool b)
        {
            bar.gameObject.SetActive(b);
            number.gameObject.SetActive(b);
        }

        public StatBar(ref Slider s, ref TMPro.TextMeshProUGUI n)
        {
            bar = s;
            number = n;
        }
    }
    public UnityEvent OnSelectUnit;

    [Header("References")]
    [SerializeField] TMPro.TextMeshProUGUI unitName = null;
    [SerializeField] StatBar healthBar = null;
    [SerializeField] TMPro.TextMeshProUGUI powerText;
    static UnitPanel _instance = null;

    Unit _selectedUnit = null;
    // Start is called before the first frame update
    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
            gameObject.SetActive(false);
        SetPanelActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_selectedUnit == null || _selectedUnit.health.IsDead())
        {
            _selectedUnit = null;
            SetPanelActive(false);
        }
    }

    void SetPanelActive(bool b)
    {
        unitName.gameObject.SetActive(b);
        healthBar.SetActive(b);
        powerText.gameObject.SetActive(b);
    }

    void _SelectUnit(Unit unit)
    {
        _selectedUnit = unit;
        unitName.text = unit.unitName;
        healthBar.number.text = (unit.health.GetHealth()).ToString();
        healthBar.bar.value = unit.health.GetHealthPercentage();
        powerText.text = unit.gun.GetBulletDamage().ToString();
        SetPanelActive(true);
        OnSelectUnit.Invoke();
    }

    public static void SelectUnit(Unit unit)
    {
        _instance._SelectUnit(unit);
    }

    private void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }
}
