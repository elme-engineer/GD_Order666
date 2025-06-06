using UnityEngine;

public class PlayBackgroundSound : MonoBehaviour
{
    [SerializeField] public AudioClip HappyBackgroundMusic;
    [SerializeField] public AudioClip SlightDmgBackgroundMusic;
    [SerializeField] public AudioClip NotificationSpamSound;
    [SerializeField] public AudioClip McDonaldsBackgroundBeeping;

    [SerializeField] public AudioSource audioSource;

    private PlayerStatus playerStatus;
    private AudioClip desiredClip = null;

    void Start()
    {
        playerStatus = GetComponentInChildren<PlayerStatus>();
        audioSource.loop = true;
    }

    void Update()
    {
        float percentage = playerStatus.DreamMeter / playerStatus.MaxDreamMeter;

        if (percentage > 0.6f)
        {
            desiredClip = HappyBackgroundMusic;
        }
        else if (percentage > 0.4f)
        {
            desiredClip = SlightDmgBackgroundMusic;
        }
        else if (percentage > 0.2f)
        {
            desiredClip = NotificationSpamSound;
        }
        else if (percentage > 0.0f)
        {
            desiredClip = McDonaldsBackgroundBeeping;
        }

        // Only switch clips if it's a different one
        if (desiredClip != null && audioSource.clip != desiredClip)
        {
            audioSource.clip = desiredClip;
            audioSource.Play();
        }
    }
}
