using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{

    [SerializeField] float fireRate = 0.1f;
    private float fireRateTimer;

    private PlayerInputActions inputActions;
    private AimManager aimManager;
    private PlayerStatus playerStatus;
    private bool isShooting = false;

    private AudioSource audioSource;
    public AudioClip shootingAudioClip;

    [Header("Bullet Properties")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform catGunMouth;
    [SerializeField] float bulletVelocity;
    [SerializeField] int bulletsPerShot;

    ParticleSystem muzzleFlashParticals;
    Light muzzleFlashLight;
    float lightIntensity;
    [SerializeField] float lightReturnSpeed = 20;

    public float damage = 20;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Shoot.started += OnShootStarted;
        inputActions.Player.Shoot.canceled += OnShootCanceled;
    }

    private void OnDisable()
    {
        inputActions.Player.Shoot.started -= OnShootStarted;
        inputActions.Player.Shoot.canceled -= OnShootCanceled;
        inputActions.Disable();
    }

    void Start()
    {
        aimManager = GetComponentInParent<AimManager>();
        fireRateTimer = fireRate;
        playerStatus = GetComponentInParent<PlayerStatus>();

        muzzleFlashLight = GetComponentInChildren<Light>();
        lightIntensity = muzzleFlashLight.intensity;
        muzzleFlashLight.intensity = 0;
        muzzleFlashParticals = GetComponentInChildren<ParticleSystem>();

        audioSource = gameObject.GetComponent<AudioSource>();

}

void Update()
    {
        fireRateTimer += Time.deltaTime;

        if (isShooting && ShouldFire())
        {
            Fire();
        }

        muzzleFlashLight.intensity = Mathf.Lerp(muzzleFlashLight.intensity, 0, lightReturnSpeed * Time.deltaTime);
    }

    void OnShootStarted(InputAction.CallbackContext context)
    {
        isShooting = true;
    }

    void OnShootCanceled(InputAction.CallbackContext context)
    {
        isShooting = false;
    }

    private bool ShouldFire()
    {
        if (!aimManager.getHasGun() || !aimManager.getIsGunMode() || !playerStatus.HasAmmo)
            return false;

        return fireRateTimer >= fireRate;
    }

    private void Fire()
    {
        fireRateTimer = 0;
        catGunMouth.LookAt(aimManager.aimPos);
        audioSource.PlayOneShot(shootingAudioClip);
        playerStatus.UseAmmo();
        TriggerMuzzleFlash();


        for (int i = 0; i < bulletsPerShot; i++)
        {
            GameObject currentBullet = Instantiate(bullet, catGunMouth.position, catGunMouth.rotation);

            BulletManager bulletManager = currentBullet.GetComponent<BulletManager>();
            bulletManager.wpManager = this;
            Rigidbody rb  = currentBullet.GetComponentInChildren<Rigidbody>();
            rb.AddForce(catGunMouth.forward * bulletVelocity, ForceMode.Impulse);
        }
    }

    void TriggerMuzzleFlash()
    {
        muzzleFlashParticals.Play();
        muzzleFlashLight.intensity = lightIntensity;
    }
}
