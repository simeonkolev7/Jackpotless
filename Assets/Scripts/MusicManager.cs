using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public AudioSource backgroundMusic; // Reference to the AudioSource playing the music
    public Slider volumeSlider; // Reference to the slider for controlling volume

    void Start()
    {
        // Initialize the slider with the current volume
        volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f); // Load saved volume or default to 1 (full volume)

        // Add listener to the slider to handle changes
        volumeSlider.onValueChanged.AddListener(UpdateVolume);

        // Set the initial volume based on the slider's value
        backgroundMusic.volume = volumeSlider.value;
    }

    // This method will update the volume when the slider changes
    private void UpdateVolume(float volume)
    {
        backgroundMusic.volume = volume;
        // Save the current volume so it persists between sessions
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }
}
