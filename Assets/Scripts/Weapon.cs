using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Weapon : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    protected string id;

    [SerializeField]
    protected bool isAutomatic;

    [SerializeField]
    private MaterialType damageType;

    [SerializeField]
    protected float damage = 10f,
            reloadTime = 1.5f,
            fireInterval = 0.1f,
            firePower = 100f,
            recoilDistance = 0.2f,
            recoilAngle = -15f,
            trailDuration = 0.2f;

    [SerializeField]
    protected int maxAmmo = 10;

    [Header("References")]
    [SerializeField]
    protected Rigidbody rb;

    [SerializeField]
    protected Transform bulletSpawn;

    [SerializeField]
    protected GameObject bulletHole;

    [SerializeField]
    private TrailRenderer bulletTrail;

    protected int currentAmmo;

    protected bool isReloading;

    protected Collider coll;

    public Action OnFire, OnReloaded;

    public string ID => id;

    public Rigidbody RB => rb;

    public int CurrentAmmo => currentAmmo;
    
    public int MaxAmmo => maxAmmo;

    public MaterialType DamageType => damageType;

    public float ReloadTime => reloadTime;

    public bool IsReloading => isReloading;

    public void Fire()
    {
        if (currentAmmo <= 0 || isReloading) return;
        if (isAutomatic)
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
        var trail = Instantiate(bulletTrail, bulletSpawn.position, bulletSpawn.rotation);
        if (Physics.Raycast(bulletSpawn.transform.position, bulletSpawn.transform.forward, out RaycastHit hit))
        {
            if(hit.collider.CompareTag("Destroyable"))
            {
                var destroyable = hit.collider.GetComponentInParent<DestroyableObject>();
                destroyable.Damage(damage, damageType);
            }
            hit.rigidbody.AddForceAtPosition(bulletSpawn.transform.forward * firePower, hit.point);
            Instantiate(bulletHole, hit.point, Quaternion.LookRotation(bulletSpawn.forward, bulletSpawn.up), hit.transform);
            trail.transform.DOMove(hit.point, trailDuration).OnComplete(() => Destroy(trail.gameObject));

        }
        else
        {
            trail.transform.DOMove(trail.transform.position + trail.transform.forward * 100f, trailDuration).OnComplete(() => Destroy(trail.gameObject));
        }

        currentAmmo--;
        Recoil();
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
        OnReloaded?.Invoke();
    }

    Tween recoil, recoilReturn, angleRecoil;
    private void Recoil()
    {
        recoil?.Kill();
        recoilReturn?.Kill();
        angleRecoil?.Kill();

        recoil = transform.DOLocalMoveZ(-recoilDistance, fireInterval/2).OnComplete(() =>  recoilReturn = transform.DOLocalMoveZ(0f, fireInterval/2));
        angleRecoil = transform.DOPunchRotation(new Vector3(recoilAngle, 0f, 0f), fireInterval / 2, 0, 0f);

    }
}
