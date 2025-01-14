using UnityEngine;

public class ExtrasSceneManager : MonoBehaviour
{
    public GameObject tutorialPanel; // Reference to the tutorial panel
    public GameObject extrasPanel;   // Reference to the main extras panel

    private void Start()
    {
        // Ensure tutorial panel is hidden at the start
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }

        // Ensure extras panel is visible at the start
        if (extrasPanel != null)
        {
            extrasPanel.SetActive(true);
        }
    }

    public void ShowTutorial()
    {
        // Hide the extras panel and show the tutorial panel
        if (extrasPanel != null) extrasPanel.SetActive(false);
        if (tutorialPanel != null) tutorialPanel.SetActive(true);
    }

    public void BackToExtras()
    {
        // Hide the tutorial panel and show the extras panel
        if (tutorialPanel != null) tutorialPanel.SetActive(false);
        if (extrasPanel != null) extrasPanel.SetActive(true);
    }
}
