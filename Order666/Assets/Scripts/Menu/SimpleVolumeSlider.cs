using UnityEngine;
using UnityEngine.UI;

public class SimpleVolumeSlider : MonoBehaviour
{
    public Slider volumeSlider;
    public AudioSource[] audioSources;

    void Start()
    {
        if (PlayerPrefs.HasKey("volume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("volume");
            volumeSlider.value = savedVolume;
            SetVolume(savedVolume);
        }
    }

    public void SetVolume(float volume)
    {
        foreach (AudioSource source in audioSources)
        {
            if (source != null)
                source.volume = volume;
        }

        PlayerPrefs.SetFloat("volume", volume);
    }
}
