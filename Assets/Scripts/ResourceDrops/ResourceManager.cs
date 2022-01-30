using TMPro;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    TMP_Text m_resourceText;

    int m_currentAmount = 10;

    public int CurrentAmount
    {
        get => m_currentAmount;
    }

    void Awake()
    {
        m_resourceText = GameObject.FindGameObjectWithTag("ResourceAmountText").GetComponent<TMP_Text>();

        UpdateUI();
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
