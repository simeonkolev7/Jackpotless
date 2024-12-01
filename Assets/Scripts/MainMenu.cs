using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    public TMP_Text LoggedInUserText;     // Text to display logged-in user info
    public GameObject mainMenuPanel;     // The main menu UI panel
    public GameObject loadingPanel;      // UI panel for the loading screen
    public TMP_Text loadingText;         // Text for loading progress
    public TMP_Text tipText;             // Text for displaying random tips
    public Button continueButton;        // Button to proceed after loading
    public string[] tips;                // Array of tips to display during loading

    private AsyncOperation operation;    // Store the AsyncOperation for scene loading
    private bool isSceneLoaded = false;  // Tracks if the scene has finished loading

    void Start()
    {
        string loggedInUser = PlayerPrefs.GetString("username", "Guest");
        UpdateLoggedInUserText(loggedInUser);

        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false); // Ensure the loading panel is hidden initially
        }

        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(false); // Ensure the Continue button is hidden initially
            continueButton.onClick.AddListener(OnContinueButtonClicked); // Assign the click listener
        }
    }

    private void UpdateLoggedInUserText(string username)
    {
        LoggedInUserText.text = "Logged in as: " + username;
    }

    public void PLAY()
    {
        StartCoroutine(LoadSceneAsync("GameScene"));
    }

    public void ACCOUNT()
    {
        StartCoroutine(LoadSceneAsync("Accounts"));
    }

    public void QUIT()
    {
        Debug.Log("Quit Game!");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Hide the main menu panel
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(false);
        }

        // Show the loading panel
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(true);
        }

        // Show a random tip
        if (tipText != null && tips.Length > 0)
        {
            tipText.text = tips[Random.Range(0, tips.Length)];
        }

        // Start loading the scene asynchronously
        operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; // Prevent the scene from activating immediately

        // Show loading progress
        if (loadingText != null)
        {
            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                loadingText.text = $"Loading... {progress * 100:F0}%";

                if (operation.progress >= 0.9f)
                {
                    isSceneLoaded = true; // Mark the scene as loaded
                    if (continueButton != null)
                    {
                        continueButton.gameObject.SetActive(true); // Show the Continue button
                    }
                    break;
                }

                yield return null;
            }
        }
    }

    private void OnContinueButtonClicked()
    {
        if (isSceneLoaded && operation != null)
        {
            operation.allowSceneActivation = true; // Activate the scene
        }
    }
}
