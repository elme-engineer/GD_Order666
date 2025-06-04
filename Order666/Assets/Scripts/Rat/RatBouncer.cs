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

    private Vector3 moveDirection;
    private float dropTimer;

    private Transform player;
    private PlayerStatus playerStatus;

    void Start()
    {
        // Choose a random flat direction
        moveDirection = Random.onUnitSphere;
        moveDirection.y = 0f;
        moveDirection.Normalize();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player != null)
            playerStatus = player.GetComponentInChildren<PlayerStatus>();

        Debug.Log("Ammo: " + playerStatus.currentAmmo);
    }


    void Update()
    {
        // Handle movement
        Vector3 nextPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;

        // Raycast to detect bounce surfaces
        if (Physics.Raycast(transform.position, moveDirection, out RaycastHit hit, bounceCheckDistance))
        {
            moveDirection = Vector3.Reflect(moveDirection, hit.normal);
            moveDirection.y = 0f; // Stay horizontal
            moveDirection.Normalize();
        }

        transform.position = nextPosition;

        // Ammo drop logic
        dropTimer += Time.deltaTime;
        if (dropTimer >= ammoDropInterval)
        {
            TryDropAmmo();
            dropTimer = 0f;
        }
    }

    void TryDropAmmo()
    {
        if (ammoPickupPrefab == null || playerStatus == null)
            return;

        if (playerStatus.currentAmmo <= playerStatus.maxAmmo / 2f)
        {
            Debug.Log("Trying to spawn ammo");

            float roll = Random.value;
            if (roll <= ammoDropChance)
            {
                Vector3 dropPosition = transform.position - moveDirection.normalized * 1.5f;
                Instantiate(ammoPickupPrefab, dropPosition, Quaternion.identity);
            }
        }
    }
}
