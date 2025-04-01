using UnityEngine;

public class GameControls : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // Find GameManager in scene
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in scene!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.PauseGame(); // Pause menu
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gameManager.DealClicked(); // Deal cards
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            gameManager.HitClicked(); // Hit (draw a card)
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameManager.StandClicked(); // Stand
        }
    }
}

