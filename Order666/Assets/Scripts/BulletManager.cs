using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] float timeToDestroy = 5f;
    [HideInInspector] public WeaponManager wpManager;

    private Rigidbody rb;
    private bool isStuck = false;
    private Vector3 initialScale;
    private Vector3 targetScale;
    private float growSpeed = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        initialScale = transform.localScale;

        // Very small final size
        targetScale = new Vector3(0.03f, 0.03f, 0.03f);

        Invoke(nameof(DestroyIfNotStuck), timeToDestroy);
    }

    void Update()
    {
        if (isStuck)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * growSpeed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hamburguer"))
        {
            HamburguerManager hbManager = collision.gameObject.GetComponentInParent<HamburguerManager>();
            hbManager.TakeDamage(wpManager.damage);
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("Plant") && !isStuck)
        {
            StickToPlant(collision);
        }
    }

    void StickToPlant(Collision collision)
    {
        isStuck = true;
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;

        transform.SetParent(collision.transform);
        transform.up = collision.contacts[0].normal;
    }

    void DestroyIfNotStuck()
    {
        if (!isStuck)
        {
            Destroy(gameObject);
        }
    }
}


