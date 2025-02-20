using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public SpriteRenderer backgroundRenderer;
    public Sprite classicBackground;
    public Sprite royalRelicBackground;
    public Sprite neonParadiseBackground;

    private void Start()
    {
        LoadBackground();
        ResizeBackground(); // Ensure the background scales correctly on start
    }

    public void SetBackground(string backgroundName)
    {
        if (backgroundRenderer == null)
        {
            Debug.LogError("BackgroundRenderer is not assigned in the Inspector!");
            return;
        }

        switch (backgroundName)
        {
            case "Classic":
                backgroundRenderer.sprite = classicBackground;
                break;
            case "RoyalRelic":
                backgroundRenderer.sprite = royalRelicBackground;
                break;
            case "NeonParadise":
                backgroundRenderer.sprite = neonParadiseBackground;
                break;
            default:
                Debug.LogError("Invalid background name: " + backgroundName);
                return;
        }

        // Save selection
        PlayerPrefs.SetString("SelectedBackground", backgroundName);
        PlayerPrefs.Save();

        Debug.Log("Background changed to: " + backgroundName);

        ResizeBackground(); // Adjust the background size when changed
    }

    private void LoadBackground()
    {
        string savedBackground = PlayerPrefs.GetString("SelectedBackground", "Classic");
        Debug.Log("Loading background: " + savedBackground);
        SetBackground(savedBackground);
    }

    private void ResizeBackground()
    {
        if (backgroundRenderer == null)
        {
            Debug.LogError("BackgroundRenderer is not assigned!");
            return;
        }

        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found!");
            return;
        }

        // Calculate screen size in world coordinates
        float worldScreenHeight = mainCamera.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight * mainCamera.aspect;

        // Get the current background sprite
        Sprite backgroundSprite = backgroundRenderer.sprite;
        if (backgroundSprite == null)
        {
            Debug.LogError("Background sprite is missing!");
            return;
        }

        float spriteWidth = backgroundSprite.bounds.size.x;
        float spriteHeight = backgroundSprite.bounds.size.y;

        // Calculate the required scale to fit the screen
        float scaleX = worldScreenWidth / spriteWidth;
        float scaleY = worldScreenHeight / spriteHeight;

        // Apply the new scale
        backgroundRenderer.transform.localScale = new Vector3(scaleX, scaleY, 1f);
    }
}
