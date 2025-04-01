using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class GameManager : MonoBehaviour
{
    // UI Elements
    public Button dealBtn;
    public Button hitBtn;
    public Button standBtn;
    public Button backToMenuBtn;
    public Button resetScoreBtn;
    public Button resumeBtn;
    public Button pauseBtn;
    public GameObject pauseMenuPanel;
    public Text pausedText;

    private int standClicks = 0;

    public PlayerScript playerScript;
    public PlayerScript dealerScript;

    public Text scoreText;
    public Text dealerScoreText;
    public Text mainText;
    public Text standBtnText;
    public Text specialCardText; // Optional UI text to show current special card

    public GameObject hideCard;

    private int winCount = 0;
    private int highScore = 0;

    public Text winCountText;
    public Text highScoreText;

    private bool isPaused = false;

    public Button cosmeticBtn;
    public GameObject cosmeticPanel;

    public SpecialCardHandler specialCardHandler;

    void Start()
    {
        dealBtn.onClick.AddListener(() => DealClicked());
        hitBtn.onClick.AddListener(() => HitClicked());
        standBtn.onClick.AddListener(() => StandClicked());
        backToMenuBtn.onClick.AddListener(() => BackToMenu());
        resetScoreBtn.onClick.AddListener(() => ResetScores());
        resumeBtn.onClick.AddListener(() => ResumeGame());
        pauseBtn.onClick.AddListener(() => PauseGame());
        cosmeticBtn.onClick.AddListener(() => ToggleCosmeticPanel());

        LoadUserData();
        cosmeticPanel.SetActive(false);

        // Ensure we have a SpecialCardHandler component
        specialCardHandler = GetComponent<SpecialCardHandler>();
        if (specialCardHandler == null)
        {
            Debug.LogWarning("Missing SpecialCardHandler, adding it dynamically.");
            specialCardHandler = gameObject.AddComponent<SpecialCardHandler>();
        }
    }

    public void DealClicked()
    {
        playerScript.ResetHand();
        dealerScript.ResetHand();
        dealerScoreText.gameObject.SetActive(false);
        mainText.gameObject.SetActive(false);
        GameObject.Find("Deck").GetComponent<DeckScript>().Shuffle();

        // Assign a special card effect for this round
        specialCardHandler.AssignNewCard();

        playerScript.StartHand();
        dealerScript.StartHand();
        scoreText.text = " " + playerScript.handValue.ToString();
        dealerScoreText.text = " " + dealerScript.handValue.ToString();
        hideCard.GetComponent<Renderer>().enabled = true;
        dealBtn.gameObject.SetActive(false);
        hitBtn.gameObject.SetActive(true);
        standBtn.gameObject.SetActive(true);
        standBtnText.text = "Stand";

        if (specialCardText != null)
        {
            var type = specialCardHandler.GetCurrentCardType();
            specialCardText.text = type switch
            {
                SpecialCardType.Limit25 => "Special Card: Limit 25!",
                SpecialCardType.Limit17 => "Special Card: Limit 17!",
                _ => "Standard Blackjack Rules"
            };
        }
    }

    public void HitClicked()
    {
        if (playerScript.cardIndex <= 10)
        {
            playerScript.GetCard();
            scoreText.text = " " + playerScript.handValue.ToString();

            int limit = specialCardHandler.GetCurrentLimit();
            if (playerScript.handValue > limit)
                RoundOver();
        }
    }

    public void StandClicked()
    {
        standClicks++;
        if (standClicks > 1)
            RoundOver();
        HitDealer();
        standBtnText.text = "Call";
    }

    public void HitDealer()
    {
        int limit = specialCardHandler.GetCurrentLimit();
        while (dealerScript.handValue < 16 && dealerScript.cardIndex < 10)
        {
            dealerScript.GetCard();
            dealerScoreText.text = " " + dealerScript.handValue.ToString();
            if (dealerScript.handValue > limit)
                RoundOver();
        }
    }

    void RoundOver()
    {
        int limit = specialCardHandler.GetCurrentLimit();

        bool playerBust = playerScript.handValue > limit;
        bool dealerBust = dealerScript.handValue > limit;
        bool playerMax = playerScript.handValue == limit;
        bool dealerMax = dealerScript.handValue == limit;

        if (standClicks < 2 && !playerBust && !dealerBust && !playerMax && !dealerMax) return;

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
        else
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

    private void UpdateWinCountUI() => winCountText.text = "Wins: " + winCount;
    private void UpdateHighScoreUI() => highScoreText.text = "High Score: " + highScore;

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
        Time.timeScale = 0f;
        StartCoroutine(BlinkPausedText());
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        StopCoroutine(BlinkPausedText());
        pausedText.gameObject.SetActive(true);
    }

    private IEnumerator BlinkPausedText()
    {
        while (isPaused)
        {
            pausedText.gameObject.SetActive(!pausedText.gameObject.activeSelf);
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

    private void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void ToggleCosmeticPanel()
    {
        bool isActive = cosmeticPanel.activeSelf;
        cosmeticPanel.SetActive(!isActive);
    }
}




