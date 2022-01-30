using TMPro;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    [SerializeField]
    float passiveResourceGainTime = 5f;

    TMP_Text m_resourceText;

    int m_currentAmount = 10;

    float m_passiveResourceTimer;

    public int CurrentAmount
    {
        get => m_currentAmount;
    }

    void Awake()
    {
        m_resourceText = GameObject.FindGameObjectWithTag("ResourceAmountText").GetComponent<TMP_Text>();

        UpdateUI();
    }

    void Update()
    {
        m_passiveResourceTimer += Time.deltaTime;

        if (m_passiveResourceTimer > passiveResourceGainTime)
        {
            m_currentAmount++;
            UpdateUI();

            m_passiveResourceTimer = 0f;
        }
    }

    public void AddResources(int a_amount = 1)
    {
        m_currentAmount += a_amount;

        UpdateUI();
    }

    public void SpendResources(int a_amount)
    {
        if (m_currentAmount < a_amount)
        {
            return;
        }

        m_currentAmount -= a_amount;

        UpdateUI();
    }

    void UpdateUI()
    {
        m_resourceText.text = m_currentAmount.ToString();
    }
}
