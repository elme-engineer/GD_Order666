using UnityEngine;

public class RatBouncer : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float bounceCheckDistance = 0.5f;

    [Header("Ammo Drop Settings")]
    public GameObject ammoPickupPrefab;
    public float ammoDropInterval = 3f;
    [Range(0f, 1f)]
    public float ammoDropChance = 0.6f;

    [Header("Bad Pickup Drops")]
    public GameObject drinkPickup;
    public float drinkDropInterval = 2f;

    private Vector3 moveDirection;
    private float ammoDropTimer;
    private float drinkDropTimer;
    private float rotationResetTimer;

    private Transform player;
    private PlayerStatus playerStatus;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        gameObject.SetActive(false);

        // Save initial transform state
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // Choose a random flat direction
        moveDirection = Random.onUnitSphere;
        moveDirection.y = 0f;
        moveDirection.Normalize();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player != null)
            playerStatus = player.GetComponentInChildren<PlayerStatus>();
    }

    void Update()
    {
        if (transform.position.y <= -15f)
        {
            transform.position = initialPosition;
        }

        // === Movement ===
        Vector3 nextPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;

        if (Physics.Raycast(transform.position, moveDirection, out RaycastHit hit, bounceCheckDistance))
        {
            moveDirection = Vector3.Reflect(moveDirection, hit.normal);
            moveDirection.y = 0f;
            moveDirection.Normalize();
        }

        transform.position = nextPosition;

        // === Timers ===
        ammoDropTimer += Time.deltaTime;
        drinkDropTimer += Time.deltaTime;
        rotationResetTimer += Time.deltaTime;

        // === Ammo drop ===
        if (ammoDropTimer >= ammoDropInterval)
        {
            TryDropAmmo();
            ammoDropTimer = 0f;
        }

        // === Bad pickup logic ===
        if (drinkDropTimer >= drinkDropInterval)
        {
            TryDropDrink();
            drinkDropTimer = 0f;
        }

        // === Reset rotation ===
        if (rotationResetTimer >= 7f)
        {
            transform.rotation = initialRotation;
            rotationResetTimer = 0f;
        }
    }

    void TryDropAmmo()
    {
        if (ammoPickupPrefab == null || playerStatus == null)
            return;

        if (playerStatus.currentAmmo <= playerStatus.maxAmmo / 2f)
        {
            float roll = Random.value;
            if (roll <= ammoDropChance)
            {
                Vector3 dropPosition = transform.position - moveDirection.normalized * 1.5f;
                Instantiate(ammoPickupPrefab, dropPosition, Quaternion.identity);
            }
        }
    }

    void TryDropDrink()
    {
        if (drinkPickup == null || playerStatus == null)
            return;

        if (playerStatus.currentAmmo > playerStatus.maxAmmo / 2)
        {
            if (Random.value <= 0.5f)
            {
                Vector3 dropPosition = transform.position + Vector3.up;
                Instantiate(drinkPickup, dropPosition, Quaternion.identity);
            }
        }
    }
}
