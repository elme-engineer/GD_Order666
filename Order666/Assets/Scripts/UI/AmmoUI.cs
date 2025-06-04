// AmmoUI.cs
using UnityEngine;
using TMPro;    // ← TextMeshPro namespace

public class AmmoUI : MonoBehaviour
{
    [Header("Drag your PlayerStatus (on the Player) here")]
    public PlayerStatus playerStatus;

    [Header("Drag your TextMeshProUGUI (AmmoText) here")]
    public TextMeshProUGUI ammoText;

    private void OnEnable()
    {
        if (playerStatus != null)
            playerStatus.onAmmoChanged.AddListener(UpdateAmmoDisplay);

        // Show the correct value immediately
        UpdateAmmoDisplay();
    }

    private void OnDisable()
    {
        if (playerStatus != null)
            playerStatus.onAmmoChanged.RemoveListener(UpdateAmmoDisplay);
    }

    private void UpdateAmmoDisplay()
    {
        if (playerStatus == null || ammoText == null) return;

        int current = playerStatus.Ammo;
        int maxAmmo = playerStatus.MaxAmmo;
        ammoText.text = $"{current} / {maxAmmo}";
    }
}
