using System.Threading;
using UnityEngine;

public class BulletManager : MonoBehaviour
{

    [SerializeField] float timeToDestroy;
    [HideInInspector] public WeaponManager wpManager;

    void Start()
    {
        Destroy(this.gameObject, timeToDestroy);
    }

    
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hamburguer"))
        {
            HamburguerManager hbManager = collision.gameObject.GetComponentInParent<HamburguerManager>();
            hbManager.TakeDamage(wpManager.damage);
            Destroy(this.gameObject);
        }
            
    }
}
