using UnityEngine;
using UnityEngine.UI;

public class SimpleVolumeSlider : MonoBehaviour
{
    public Slider volumeSlider;
    public AudioSource audioSource; // Optional, if you're directly adjusting one sound source

    void Start()
    {
        // Set initial value
        volumeSlider.value = 0.5f;

        // Subscribe to slider changes
        volumeSlider.onValueChanged.AddListener(SetVolume);
        
        // Optional: apply saved value
        // float savedVolume = PlayerPrefs.GetFloat("volume", 0.5f);
        // volumeSlider.value = savedVolume;
        // SetVolume(savedVolume);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume; // Master volume
        // OR, for a specific source:
        // if (audioSource != null) audioSource.volume = volume;

        // Optional: Save volume between sessions
        // PlayerPrefs.SetFloat("volume", volume);
    }
}
