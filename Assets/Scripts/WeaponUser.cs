using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class WeaponUser : MonoBehaviour
{
    [SerializeField]
    private WeaponHUD weaponHUD;

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    private float weaponThrowForce = 10f,
        pickUpDistance = 5f;

    [SerializeField]
    private LayerMask pickUpLayerMask;

    private Weapon currentWeapon;

    Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void DropWeapon()
    {
        if (currentWeapon == null) return;
        currentWeapon.transform.parent = null;
        currentWeapon.TogglePhysics(true);
        currentWeapon.RB.AddForce(transform.forward * weaponThrowForce, ForceMode.Impulse);
        currentWeapon.OnFire = null;
        currentWeapon.OnReloaded = null;
        weaponHUD.Toggle(false);

        currentWeapon = null;
    }

    public void TryPickUpWeapon()
    {
        if(Physics.SphereCast(mainCamera.transform.position, 0.2f, mainCamera.transform.forward, out RaycastHit hit, pickUpDistance, pickUpLayerMask))
        {
            if(hit.collider.CompareTag("Weapon"))
            {
                currentWeapon = hit.collider.GetComponentInParent<Weapon>();
                currentWeapon.transform.parent = weaponHolder;
                currentWeapon.TogglePhysics(false);
                currentWeapon.transform.SetPositionAndRotation(weaponHolder.position, weaponHolder.rotation);
                weaponHUD.Toggle(true);
                weaponHUD.DisplayAmmoCount(currentWeapon.CurrentAmmo, currentWeapon.MaxAmmo);
                weaponHUD.SetWeaponDescription(currentWeapon.ID, currentWeapon.DamageType.ToString());

                currentWeapon.OnFire = null;
                currentWeapon.OnFire += () => OnWeaponFired();
                currentWeapon.OnReloaded = null;
                currentWeapon.OnReloaded += () => OnWeaponReloaded();

            }
        }

    }

    private void OnWeaponFired()
    {
        weaponHUD.DisplayAmmoCount(currentWeapon.CurrentAmmo, currentWeapon.MaxAmmo);
    }

    private void OnWeaponReloaded()
    {
        weaponHUD.DisplayAmmoCount(currentWeapon.CurrentAmmo, currentWeapon.MaxAmmo);
        weaponHUD.ToggleReloadSlider(false);

    }

    private void OnInteraction(InputValue value)
    {
        if (!value.isPressed) return;
        if(currentWeapon)
        {
            DropWeapon();
        }
        else
        {
            TryPickUpWeapon();
        }
    }

    private void OnShoot(InputValue value)
    {
        if(currentWeapon != null)
        {
            if (value.isPressed)
            {
                currentWeapon.Fire();
            }
            else
            {
                currentWeapon.StopFire();
            }

        }
    }

    private void OnReload(InputValue value)
    {
        if (currentWeapon != null)
        {
            if (value.isPressed && currentWeapon.IsReloading == false)
            {
                currentWeapon.Reload();
                weaponHUD.ToggleReloadSlider(true);
                weaponHUD.DisplayReloadTime(currentWeapon.ReloadTime);
            }
        }
    }
}
