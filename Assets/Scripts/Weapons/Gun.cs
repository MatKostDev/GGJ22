using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] int poolSize = 2;
    [SerializeField] float bulletSpeed = 5.0f;

    [Header("References")]
    [SerializeField] GameObject bulletPrefab = null;
    [SerializeField] List<Transform> bulletEmitterPoints = new List<Transform>();

    List<GameObject> _bulletPool = new List<GameObject>();
    int _bulletKey = 0;
    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            _bulletPool.Add(GameObject.Instantiate(bulletPrefab));
            _bulletPool[_bulletPool.Count - 1].SetActive(false);
        }
    }
    public override void Attack()
    {
        base.Attack();
        foreach (var bep in bulletEmitterPoints)
        {
            GameObject bullet = _bulletPool[_bulletKey];
            //foreach (var b in _bulletPool)
            //    if (!b.activeSelf)
            //        bullet = b;
            //if (bullet == null)
            //    bullet = _bulletPool[_bulletPool.Count - 1];

            bullet.SetActive(true);
            bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
            bullet.transform.rotation = bep.transform.rotation;
            bullet.transform.position = bep.transform.position;

            var dir = bep.transform.forward;
            dir = dir.normalized;
            bullet.GetComponent<Rigidbody>().AddForce(dir * bulletSpeed, ForceMode.Impulse);
            _bulletKey = (_bulletKey + 1) % _bulletPool.Count;
        }
    }

    public float GetBulletDamage()
    {
        return _bulletPool[0].GetComponent<DamageDoer>().damage;
    }
}
