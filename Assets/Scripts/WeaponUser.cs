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
        currentWeapon.transform.parent = null;
        currentWeapon.TogglePhysics(true);
        currentWeapon.RB.AddForce(transform.forward * weaponThrowForce, ForceMode.Impulse);
        currentWeapon.OnFire = null;
        weaponHUD.Toggle(false);


    }

    public void TryPickUpWeapon()
    {
        if(Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, pickUpDistance, pickUpLayerMask))
        {
            if(hit.collider.CompareTag("Weapon"))
            {
                currentWeapon = hit.collider.GetComponentInParent<Weapon>();
                currentWeapon.transform.parent = weaponHolder;
                currentWeapon.TogglePhysics(false);
                currentWeapon.transform.SetPositionAndRotation(weaponHolder.position, weaponHolder.rotation);
                weaponHUD.Toggle(true);
                weaponHUD.DisplayAmmoCount(currentWeapon.CurrentAmmo, currentWeapon.MaxAmmo);

                currentWeapon.OnFire = null;
                currentWeapon.OnFire += () => weaponHUD.DisplayAmmoCount(currentWeapon.CurrentAmmo, currentWeapon.MaxAmmo);
            }
        }

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
            if (value.isPressed)
            {
                currentWeapon.Reload();
            }
        }
    }
}
