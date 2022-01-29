using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

public class ResourceDrop : MonoBehaviour
{
    public UnityEvent onCollected = new UnityEvent();

    const float MAX_SPEED = 5f;
    const float FLY_SPEED = 15f;

    Rigidbody m_rigidbody;

    bool m_isFlyingToPlayer = false;

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (m_rigidbody.velocity.sqrMagnitude > MAX_SPEED * MAX_SPEED)
        {
            m_rigidbody.velocity = m_rigidbody.velocity.normalized * MAX_SPEED;
        }
    }

    public void OnEmitted()
    {
        SetRandomVelocity();
    }

    void SetRandomVelocity()
    {
        m_rigidbody.velocity =
            new Vector3(
                Random.Range(-MAX_SPEED, MAX_SPEED),
                Random.Range(-MAX_SPEED, MAX_SPEED),
                Random.Range(-MAX_SPEED, MAX_SPEED));
    }

    void OnTriggerEnter(Collider a_other)
    {
        if (a_other.transform.CompareTag("LootCollector"))
        {
            if (m_isFlyingToPlayer)
            {
                return;
            }
            StartCoroutine(FlyToPlayerRoutine(a_other.transform));
        }
    }

    IEnumerator FlyToPlayerRoutine(Transform a_collectorTransform)
    {
        m_isFlyingToPlayer = true;

        GetComponent<Collider>().enabled = false;

        m_rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

        Vector3 playerVector = a_collectorTransform.position - transform.position;

        while (playerVector.magnitude > 0.2f)
        {
            playerVector = a_collectorTransform.position - transform.position;

            m_rigidbody.velocity = playerVector.normalized * FLY_SPEED;

            yield return null;
        }

        onCollected?.Invoke();

        Destroy(gameObject);
    }
}
