using UnityEngine;

public class HamburguerManager : MonoBehaviour
{

    [SerializeField] float hamburguerHp;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        if(hamburguerHp > 0)
        {
            hamburguerHp -= damage;
        }else
            Death();

    }

    void Death()
    {

        Destroy(this.gameObject);
    }
}
