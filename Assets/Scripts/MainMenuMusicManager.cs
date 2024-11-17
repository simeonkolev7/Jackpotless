using UnityEngine;
using UnityEngine.UI;

public class MainMenuMusicManager : MonoBehaviour
{
    public AudioSource backgroundMusic; // Reference to the AudioSource for main menu music
    public Slider volumeSlider; // Slider for controlling music volume

    private void Start()
    {
        // Load the saved music volume or set it to default (1.0)
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);

        // Set the volume of the AudioSource
        backgroundMusic.volume = savedVolume;

        // Initialize the slider's value
        volumeSlider.value = savedVolume;

        // Add a listener to the slider for handling volume changes
        volumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
    }

    private void UpdateMusicVolume(float volume)
    {
        // Update the AudioSource's volume
        backgroundMusic.volume = volume;

        // Save the volume setting for persistence across sessions
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }
}

