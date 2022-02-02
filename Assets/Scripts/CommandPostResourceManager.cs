using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandPostResourceManager : MonoBehaviour
{
    [SerializeField]
    float dropCooldown = 10f;

    [SerializeField]
    CommandPostResourceDropper initialDrop;

    CommandPostResourceDropper[] m_dropEmitters;

    float m_dropCountdown;

    void Awake()
    {
        m_dropEmitters = FindObjectsOfType<CommandPostResourceDropper>();
    }

    void Start()
    {
        if (initialDrop)
        {
            initialDrop.EmitLoot(false);
            m_dropCountdown = dropCooldown;
        }
    }

    void Update()
    {
        m_dropCountdown -= Time.deltaTime;

        if (m_dropCountdown < 0f)
        {
            int randomIndex = Random.Range(0, m_dropEmitters.Length);
            m_dropEmitters[randomIndex].EmitLoot(false);

            m_dropCountdown = dropCooldown;
        }
    }
}
