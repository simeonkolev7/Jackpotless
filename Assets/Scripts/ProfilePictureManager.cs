using UnityEngine;
using UnityEngine.UI;

public class ProfilePictureManager : MonoBehaviour
{
    public Image profilePicture; // UI Image for profile picture
    public Sprite defaultPicture;
    public Sprite monaka;
    public Sprite chinaCat;
    public Sprite ballerCat;

    public Button profilePictureBtn; // Button to open the profile picture panel
    public Button closeProfilePanelBtn; // Button to close the panel
    public GameObject profilePanel; // The panel for selecting profile pictures

    private void Start()
    {
        LoadProfilePicture();

        // Add listeners for buttons
        profilePictureBtn.onClick.AddListener(() => ToggleProfilePanel(true));
        closeProfilePanelBtn.onClick.AddListener(() => ToggleProfilePanel(false));

        // Hide the panel on start
        profilePanel.SetActive(false);
    }

    public void SetProfilePicture(string pictureName)
    {
        if (profilePicture == null)
        {
            Debug.LogError("ProfilePicture UI Image is not assigned in the Inspector!");
            return;
        }

        switch (pictureName)
        {
            case "Default":
                profilePicture.sprite = defaultPicture;
                break;
            case "Monaka":
                profilePicture.sprite = monaka;
                break;
            case "ChinaCat":
                profilePicture.sprite = chinaCat;
                break;
            case "BallerCat":
                profilePicture.sprite = ballerCat;
                break;
            default:
                Debug.LogError("Invalid profile picture name: " + pictureName);
                return;
        }

        PlayerPrefs.SetString("SelectedProfilePicture", pictureName);
        PlayerPrefs.Save();

        Debug.Log("Profile picture changed to: " + pictureName);
    }

    private void LoadProfilePicture()
    {
        string savedPicture = PlayerPrefs.GetString("SelectedProfilePicture", "Default");
        SetProfilePicture(savedPicture);
    }

    private void ToggleProfilePanel(bool isOpen)
    {
        profilePanel.SetActive(isOpen);
    }
}

