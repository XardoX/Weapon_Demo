using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponHUD : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ammoCount;

    [SerializeField]
    private CanvasGroup canvasGroup;

    public void Toggle(bool value)
    {
        var alpha = value ? 1f : 0f;
        canvasGroup.alpha = alpha;
    }

    public void DisplayAmmoCount(int currentAmmo, int maxAmmo)
    {
        ammoCount.text = $"{currentAmmo}/{maxAmmo}";
    }
}
