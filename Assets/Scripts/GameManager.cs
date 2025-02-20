using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // UI Elements
    public Button dealBtn;
    public Button hitBtn;
    public Button standBtn;
    public Button backToMenuBtn;
    public Button resetScoreBtn;
    public Button resumeBtn;
    public Button pauseBtn; // Pause button
    public GameObject pauseMenuPanel;
    public Text pausedText;

    private int standClicks = 0;

    // Access the player and dealer's script
    public PlayerScript playerScript;
    public PlayerScript dealerScript;

    // Text elements for HUD
    public Text scoreText;
    public Text dealerScoreText;
    public Text mainText;
    public Text standBtnText;

    // Card hiding dealer's 2nd card
    public GameObject hideCard;

    // Highscore and Win Count
    private int winCount = 0;
    private int highScore = 0;

    // Text elements to display win count and high score
    public Text winCountText;
    public Text highScoreText;

    private bool isPaused = false;

    // Cosmetic UI Elements
    public Button cosmeticBtn; // Button to open the cosmetic panel
    public GameObject cosmeticPanel; // Panel for cosmetic options

    void Start()
    {
        // Add on click listeners to the buttons
        dealBtn.onClick.AddListener(() => DealClicked());
        hitBtn.onClick.AddListener(() => HitClicked());
        standBtn.onClick.AddListener(() => StandClicked());
        backToMenuBtn.onClick.AddListener(() => BackToMenu());
        resetScoreBtn.onClick.AddListener(() => ResetScores());
        resumeBtn.onClick.AddListener(() => ResumeGame());
        pauseBtn.onClick.AddListener(() => PauseGame()); // Listener for Pause button

        // Add listener for cosmetic button
        cosmeticBtn.onClick.AddListener(() => ToggleCosmeticPanel());

        // Load the saved win count and high score
        LoadUserData();

        // Hide the cosmetic panel on start
        cosmeticPanel.SetActive(false);
    }

    private void DealClicked()
    {
        playerScript.ResetHand();
        dealerScript.ResetHand();
        dealerScoreText.gameObject.SetActive(false);
        mainText.gameObject.SetActive(false);
        GameObject.Find("Deck").GetComponent<DeckScript>().Shuffle();
        playerScript.StartHand();
        dealerScript.StartHand();
        scoreText.text = " " + playerScript.handValue.ToString();
        dealerScoreText.text = " " + dealerScript.handValue.ToString();
        hideCard.GetComponent<Renderer>().enabled = true;
        dealBtn.gameObject.SetActive(false);
        hitBtn.gameObject.SetActive(true);
        standBtn.gameObject.SetActive(true);
        standBtnText.text = "Stand";
    }

    private void HitClicked()
    {
        if (playerScript.cardIndex <= 10)
        {
            playerScript.GetCard();
            scoreText.text = " " + playerScript.handValue.ToString();
            if (playerScript.handValue > 20) RoundOver();
        }
    }

    private void StandClicked()
    {
        standClicks++;
        if (standClicks > 1) RoundOver();
        HitDealer();
        standBtnText.text = "Call";
    }

    private void HitDealer()
    {
        while (dealerScript.handValue < 16 && dealerScript.cardIndex < 10)
        {
            dealerScript.GetCard();
            dealerScoreText.text = " " + dealerScript.handValue.ToString();
            if (dealerScript.handValue > 20) RoundOver();
        }
    }

    void RoundOver()
    {
        bool playerBust = playerScript.handValue > 21;
        bool dealerBust = dealerScript.handValue > 21;
        bool player21 = playerScript.handValue == 21;
        bool dealer21 = dealerScript.handValue == 21;

        if (standClicks < 2 && !playerBust && !dealerBust && !player21 && !dealer21) return;

        if (playerBust && dealerBust)
        {
            mainText.text = "All Bust: No winners";
        }
        else if (playerBust || (!dealerBust && dealerScript.handValue > playerScript.handValue))
        {
            mainText.text = "Dealer wins!";
        }
        else if (dealerBust || playerScript.handValue > dealerScript.handValue)
        {
            mainText.text = "You win!";
            winCount++;
            highScore += 10;
            SaveUserData();
        }
        else if (playerScript.handValue == dealerScript.handValue)
        {
            mainText.text = "Push: It's a tie!";
        }

        hitBtn.gameObject.SetActive(false);
        standBtn.gameObject.SetActive(false);
        dealBtn.gameObject.SetActive(true);
        mainText.gameObject.SetActive(true);
        dealerScoreText.gameObject.SetActive(true);
        hideCard.GetComponent<Renderer>().enabled = false;
        standClicks = 0;
    }

    private void UpdateWinCountUI()
    {
        winCountText.text = "Wins: " + winCount.ToString();
    }

    private void UpdateHighScoreUI()
    {
        highScoreText.text = "High Score: " + highScore.ToString();
    }

    private void SaveUserData()
    {
        PlayerPrefs.SetInt("WinCount", winCount);
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
        UpdateWinCountUI();
        UpdateHighScoreUI();
    }

    private void LoadUserData()
    {
        winCount = PlayerPrefs.GetInt("WinCount", 0);
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateWinCountUI();
        UpdateHighScoreUI();
    }

    public void ResetScores()
    {
        winCount = 0;
        highScore = 0;
        PlayerPrefs.SetInt("WinCount", winCount);
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
        UpdateWinCountUI();
        UpdateHighScoreUI();
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f; // Pause game time
        StartCoroutine(BlinkPausedText());
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f; // Resume game time
        StopCoroutine(BlinkPausedText());
        pausedText.gameObject.SetActive(true); // Ensure text is visible when unpaused
    }

    private IEnumerator BlinkPausedText()
    {
        while (isPaused)
        {
            pausedText.gameObject.SetActive(!pausedText.gameObject.activeSelf); // Toggle visibility
            yield return new WaitForSecondsRealtime(0.5f); // Use unscaled time
        }
    }

    private void BackToMenu()
    {
        Time.timeScale = 1f; // Ensure time scale is reset
        SceneManager.LoadScene("MainMenu");
    }

    public void ToggleCosmeticPanel()
    {
        // Toggle the cosmetic panel visibility
        bool isActive = cosmeticPanel.activeSelf;
        cosmeticPanel.SetActive(!isActive);
    }
}





