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
    }

    private void LoadBackground()
    {
        string savedBackground = PlayerPrefs.GetString("SelectedBackground", "Classic");
        Debug.Log("Loading background: " + savedBackground);
        SetBackground(savedBackground);
    }
}
