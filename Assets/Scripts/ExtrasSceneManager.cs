using UnityEngine;
using UnityEngine.SceneManagement;

public class ExtrasSceneManager : MonoBehaviour
{
    public GameObject extrasPanel;       // Reference to the main Extras Panel
    public GameObject tutorialPanel;    // Reference to the Tutorial Panel
    public GameObject achievementsPanel; // Reference to the Achievements Panel
    public GameObject cosmeticsPanel;   // Reference to the Cosmetics Panel

    private void Start()
    {
        // Ensure all panels are hidden except for the Extras Panel
        if (extrasPanel != null) extrasPanel.SetActive(true);
        if (tutorialPanel != null) tutorialPanel.SetActive(false);
        if (achievementsPanel != null) achievementsPanel.SetActive(false);
        if (cosmeticsPanel != null) cosmeticsPanel.SetActive(false);
    }

    public void ShowTutorial()
    {
        // Show Tutorial Panel and hide Extras Panel
        if (extrasPanel != null) extrasPanel.SetActive(false);
        if (tutorialPanel != null) tutorialPanel.SetActive(true);
    }

    public void ShowAchievements()
    {
        // Show Achievements Panel and hide Extras Panel
        if (extrasPanel != null) extrasPanel.SetActive(false);
        if (achievementsPanel != null) achievementsPanel.SetActive(true);
    }

    public void ShowCosmetics()
    {
        // Show Cosmetics Panel and hide Extras Panel
        if (extrasPanel != null) extrasPanel.SetActive(false);
        if (cosmeticsPanel != null) cosmeticsPanel.SetActive(true);
    }

    public void BackToExtras()
    {
        // Show Extras Panel and hide all other panels
        if (extrasPanel != null) extrasPanel.SetActive(true);
        if (tutorialPanel != null) tutorialPanel.SetActive(false);
        if (achievementsPanel != null) achievementsPanel.SetActive(false);
        if (cosmeticsPanel != null) cosmeticsPanel.SetActive(false);
    }

    public void BackToMainMenu()
    {
        // Load the Main Menu scene
        SceneManager.LoadScene("MainMenu");
    }
}
