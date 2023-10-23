using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class WeaponHUD : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ammoCount,
        weaponDescriptionText;

    [SerializeField]
    private Slider reloadSlider;

    [SerializeField]
    private CanvasGroup canvasGroup;

    public void Toggle(bool value)
    {
        var alpha = value ? 1f : 0f;
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, alpha, 0.5f);
    }

    public void SetWeaponDescription(string weaponName, string whatDamages)
    {
        weaponDescriptionText.text = $"Weapon: {weaponName}\nDamages: {whatDamages}";
    }

    public void DisplayAmmoCount(int currentAmmo, int maxAmmo)
    {
        ammoCount.text = $"{currentAmmo}/{maxAmmo}";
    }

    public void ToggleReloadSlider(bool toggle) => reloadSlider.gameObject.SetActive(toggle);

    public void DisplayReloadTime(float reloadDuration)
    {
        reloadSlider.value = 0;
        reloadSlider.maxValue = reloadDuration;
        reloadSlider.DOValue(reloadDuration, reloadDuration);
    }
}
