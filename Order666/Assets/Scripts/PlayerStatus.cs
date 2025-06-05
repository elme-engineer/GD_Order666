using UnityEngine;
using UnityEngine.Events;

public class PlayerStatus : MonoBehaviour
{
    [Header("Dream Meter (like HP)")]
    [SerializeField] private float maxDreamMeter = 100f;
    [SerializeField] private float currentDreamMeter;

    [Header("Ammo")]
    [SerializeField] public int maxAmmo = 60;
    [SerializeField] public int currentAmmo = 30;

    [Header("Events")]
    public UnityEvent onDreamMeterChanged;
    public UnityEvent onDreamBroken;
    public UnityEvent onAmmoChanged;

    private void Awake()
    {
        currentDreamMeter = maxDreamMeter;
    }

    public float DreamMeter => currentDreamMeter;
    public float MaxDreamMeter => maxDreamMeter;

    public void TakeDreamDamage(float amount)
    {
        Debug.Log("RECEIVED " + amount + " DMG!");
        currentDreamMeter = Mathf.Max(currentDreamMeter - amount, 0f);

        Debug.Log("DreamMeter: " + currentDreamMeter);
        onDreamMeterChanged?.Invoke();

        if (currentDreamMeter <= 0f)
        {
            onDreamBroken?.Invoke(); // could trigger wake-up or game over
        }
    }

    public void RestoreDream(float amount)
    {
        currentDreamMeter = Mathf.Min(currentDreamMeter + amount, maxDreamMeter);
        onDreamMeterChanged?.Invoke();
    }

    public void FullyRestoreDream() => RestoreDream(maxDreamMeter);

    public bool IsDreamBroken => currentDreamMeter <= 0f;

    public int Ammo => currentAmmo;
    public int MaxAmmo => maxAmmo;

    public bool HasAmmo => currentAmmo > 0;

    public void UseAmmo(int amount = 1)
    {
        if (currentAmmo >= amount)
        {
            currentAmmo -= amount;
            onAmmoChanged?.Invoke();
        }
    }

    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
        onAmmoChanged?.Invoke();
    }

    public void Reload() => AddAmmo(maxAmmo);
}
