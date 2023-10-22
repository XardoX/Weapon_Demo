using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class WeaponUser : MonoBehaviour
{
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
        currentWeapon.RB.isKinematic = false;
        currentWeapon.RB.AddForce(transform.forward * weaponThrowForce, ForceMode.Impulse);

    }

    public void TryPickUpWeapon()
    {
        if(Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, pickUpDistance, pickUpLayerMask))
        {
            if(hit.collider.CompareTag("Weapon"))
            {
                currentWeapon = hit.collider.GetComponentInParent<Weapon>();
                currentWeapon.transform.parent = weaponHolder;
                currentWeapon.RB.isKinematic = true;
                currentWeapon.transform.SetPositionAndRotation(weaponHolder.position, weaponHolder.rotation);
            }
        }

    }

    public void OnInteraction(InputValue value)
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
}
