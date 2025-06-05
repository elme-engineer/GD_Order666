using UnityEngine;

public class HBBulletManager : MonoBehaviour
{

    [SerializeField] float timeToDestroy;
    [HideInInspector] public HamburguerManager hbManager;
    private PlayerStatus playerStatus;
    [SerializeField] AudioClip dmgAudioClip;
    [SerializeField] AudioSource audioSource;

    void Start()
    {
        Destroy(this.gameObject, timeToDestroy);
    }


    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioSource.PlayOneShot(dmgAudioClip);
        }

    }
}
