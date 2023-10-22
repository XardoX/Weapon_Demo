using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    protected Rigidbody rb;

    [SerializeField]
    protected Transform bulletSpawn;

    [SerializeField]
    protected Bullet bulletPrefab;

    [SerializeField]
    protected bool isAutomatic, 
              isHitscan;

    [SerializeField]
    protected float reloadTime = 1.5f, 
            fireInterval = 0.1f,
            firePower = 100f;

    [SerializeField]
    protected int maxAmmo = 10;

    protected int currentAmmo;

    protected bool isReloading;

    protected Collider coll;

    public Action OnFire;

    public Rigidbody RB => rb;

    public int CurrentAmmo => currentAmmo;
    
    public int MaxAmmo => maxAmmo;

    private void Awake()
    {
        coll = GetComponentInChildren<Collider>();
    }

    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    private void FireBullet()
    {
        if(isHitscan) 
        {
            if (Physics.Raycast(bulletSpawn.transform.position, bulletSpawn.transform.forward, out RaycastHit hit))
            {
                hit.rigidbody?.AddForceAtPosition(bulletSpawn.transform.forward * firePower, hit.point);
            }
        }
        else
        {
            var newBullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            newBullet.Fire(firePower);

        }
        currentAmmo--;
        OnFire?.Invoke();
    }

    private IEnumerator AutomaticFire()
    {
        var waitFireInterval = new WaitForSeconds(fireInterval);
        while (currentAmmo > 0)
        {
            FireBullet();
            yield return waitFireInterval;
        }
    }

    private IEnumerator StartReload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
        currentAmmo = maxAmmo;
    }

    public void Fire()
    {
        if(isAutomatic)
        {
            StartCoroutine(AutomaticFire());
        }
        else
        {
            FireBullet();
        }
    }

    public void StopFire()
    {
        StopAllCoroutines();
    }

    public void Reload()
    {
        if (isReloading) return;
        StopAllCoroutines();
        StartCoroutine(StartReload());
    }

    public void TogglePhysics(bool toggle)
    {
        rb.isKinematic = !toggle;
        coll.enabled = toggle;
    }

}
