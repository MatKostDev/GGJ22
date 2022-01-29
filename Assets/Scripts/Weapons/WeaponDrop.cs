using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponDrop : MonoBehaviour
{
    [SerializeField] float suctionScalar = 1.0f;
    [SerializeField] AnimationCurve punchCurve;
    [SerializeField] float lerpSpeed;
    [SerializeField] UnityEvent OnPickup = null;

    [Header("References")]
    [SerializeField] new Rigidbody rigidbody = null;
    [SerializeField] Transform lerpPoint = null;
    [SerializeField] GameObject parentObject = null;
    private Vector3 _orignalPosition;

    Coroutine c = null;

    IEnumerator Lerp()
    {
        while (true)
        {
            float x = 0.0f;
            while (x < 1.0f)
            {
                yield return new WaitForEndOfFrame();
                x += Time.deltaTime * lerpSpeed;
                transform.localPosition = Vector3.Lerp(_orignalPosition, lerpPoint.localPosition, punchCurve.Evaluate(x));
            }
            x = 1.0f;
            while (x > 0.0f)
            {
                yield return new WaitForEndOfFrame();
                x -= Time.deltaTime * lerpSpeed;
                transform.localPosition = Vector3.Lerp(_orignalPosition, lerpPoint.localPosition, punchCurve.Evaluate(x));
            }
            x = 0.0f;
        }
    }
    private void OnEnable()
    {
        _orignalPosition = transform.localPosition;
        c = StartCoroutine(Lerp());
    }

    private void OnDisable()
    {
        transform.localPosition = _orignalPosition;
        rigidbody.velocity = Vector3.zero;
    }

    private void Update() {
        transform.Rotate(new Vector3(0.0f,1.0f,0.0f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        var dir = other.gameObject.transform.position - transform.position;
        dir = dir.normalized;

        rigidbody.AddForce(dir * suctionScalar, ForceMode.Impulse);
        StopCoroutine(c);
        var pc = other.transform.parent.gameObject;
        var wm = pc.GetComponentInChildren<WeaponManager>();

        OnPickup.Invoke();
        IEnumerator Wait()
        {
            yield return new WaitForSeconds(0.5f);
            wm.ActivateSecondaryWeapon();
            Destroy(parentObject);
        }
        StartCoroutine(Wait());
    }
}
