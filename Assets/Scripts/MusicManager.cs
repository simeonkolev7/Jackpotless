using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public AudioSource backgroundMusic; // Reference to the AudioSource for background music
    public AudioSource soundEffectsSource; // Reference to the AudioSource for sound effects
    public Slider musicVolumeSlider; // Slider for controlling music volume
    public Slider soundEffectsVolumeSlider; // Slider for controlling sound effects volume

    void Start()
    {
        // Load saved volumes or set defaults
        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float savedSoundEffectsVolume = PlayerPrefs.GetFloat("SoundEffectsVolume", 1f);

        // Set initial slider values
        musicVolumeSlider.value = savedMusicVolume;
        soundEffectsVolumeSlider.value = savedSoundEffectsVolume;

        // Set initial volumes for audio sources
        backgroundMusic.volume = savedMusicVolume;
        soundEffectsSource.volume = savedSoundEffectsVolume;

        // Add listeners for slider changes
        musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
        soundEffectsVolumeSlider.onValueChanged.AddListener(UpdateSoundEffectsVolume);
    }

    // Update the music volume and save it
    private void UpdateMusicVolume(float volume)
    {
        backgroundMusic.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    // Update the sound effects volume and save it
    private void UpdateSoundEffectsVolume(float volume)
    {
        soundEffectsSource.volume = volume;
        PlayerPrefs.SetFloat("SoundEffectsVolume", volume);
        PlayerPrefs.Save();
    }
}

