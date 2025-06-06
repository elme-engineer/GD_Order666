using UnityEngine;

public class HamburguerManager : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float hamburguerHp = 10f;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float fireInterval = 1f;

    [Header("Combat")]
    [SerializeField] Transform bulletStart;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 100f;
    [SerializeField] public float damage = 10f;

    [Header("Behavior")]
    [SerializeField] float minHeight = 5f;
    [SerializeField] float minDistanceToPlayer = 2f;

    private Transform player;
    private float fireTimer;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Player not found! Did you forget to tag the player as 'Player'?");
        }

        fireTimer = fireInterval;
    }

    void Update()
    {
        if (player == null) return;

        // === Move toward player only if farther than min distance ===
        Vector3 directionToPlayer = (player.position - transform.position);
        float distance = directionToPlayer.magnitude;

        if (distance > minDistanceToPlayer)
        {
            Vector3 moveDir = directionToPlayer.normalized;
            transform.position += moveDir * moveSpeed * Time.deltaTime;

            // Clamp height so it never goes below minHeight
            Vector3 clampedPos = transform.position;
            clampedPos.y = Mathf.Max(clampedPos.y, minHeight);
            transform.position = clampedPos;
        }

        // === Shoot at Player ===
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireInterval;
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null && bulletStart != null)
        {
            GameObject currentBullet = Instantiate(bulletPrefab, bulletStart.position, Quaternion.identity);

            HBBulletManager hbBulletManager = currentBullet.GetComponent<HBBulletManager>();
            hbBulletManager.hbManager = this;

            Rigidbody rb = currentBullet.GetComponentInChildren<Rigidbody>();
            if (rb != null && player != null)
            {
                Vector3 shootDirection = (player.position - bulletStart.position).normalized;
                rb.AddForce(shootDirection * bulletSpeed, ForceMode.Impulse);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (hamburguerHp > 0)
        {
            hamburguerHp -= damage;
            Debug.Log("Hamburguer took damage!");
        }
        else
        {
            Death();
        }
    }

    void Death()
    {
        Destroy(this.gameObject);
    }
}
