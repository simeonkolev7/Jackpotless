using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Add this to use SceneManager

public class GameManager : MonoBehaviour
{
    // Game Buttons
    public Button dealBtn;
    public Button hitBtn;
    public Button standBtn;
    public Button backToMenuBtn; // New button for returning to the main menu
    public Button resetScoreBtn; // Button for resetting scores

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
    private int winCount = 0;  // Tracks how many rounds the player has won
    private int highScore = 0; // Tracks the player's highest score

    // Text elements to display win count and high score
    public Text winCountText;
    public Text highScoreText;

    void Start()
    {
        // Add on click listeners to the buttons
        dealBtn.onClick.AddListener(() => DealClicked());
        hitBtn.onClick.AddListener(() => HitClicked());
        standBtn.onClick.AddListener(() => StandClicked());
        backToMenuBtn.onClick.AddListener(() => BackToMenu()); // Add listener for back button
        resetScoreBtn.onClick.AddListener(() => ResetScores()); // Add listener for reset button

        // Load the saved win count and high score
        LoadUserData();
    }

    private void DealClicked()
    {
        // Reset round, hide text, prep for new hand
        playerScript.ResetHand();
        dealerScript.ResetHand();
        dealerScoreText.gameObject.SetActive(false);
        mainText.gameObject.SetActive(false);
        GameObject.Find("Deck").GetComponent<DeckScript>().Shuffle();
        playerScript.StartHand();
        dealerScript.StartHand();
        scoreText.text = "Hand: " + playerScript.handValue.ToString();
        dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
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
            scoreText.text = "Hand: " + playerScript.handValue.ToString();
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
            dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
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

        bool roundOver = true;
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
            SaveUserData(); // Save score and win count to PlayerPrefs
        }
        else if (playerScript.handValue == dealerScript.handValue)
        {
            mainText.text = "Push: It's a tie!";
        }
        else
        {
            roundOver = false;
        }

        if (roundOver)
        {
            hitBtn.gameObject.SetActive(false);
            standBtn.gameObject.SetActive(false);
            dealBtn.gameObject.SetActive(true);
            mainText.gameObject.SetActive(true);
            dealerScoreText.gameObject.SetActive(true);
            hideCard.GetComponent<Renderer>().enabled = false;
            standClicks = 0;
        }
    }

    private void UpdateWinCountUI()
    {
        winCountText.text = "Wins: " + winCount.ToString();
    }

    private void UpdateHighScoreUI()
    {
        highScoreText.text = "High Score: " + highScore.ToString();
    }

    // Method to save user data
    private void SaveUserData()
    {
        PlayerPrefs.SetInt("WinCount", winCount);
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
        UpdateWinCountUI();
        UpdateHighScoreUI();
    }

    // Method to load user data
    private void LoadUserData()
    {
        winCount = PlayerPrefs.GetInt("WinCount", 0);
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateWinCountUI();
        UpdateHighScoreUI();
    }

    public void ResetScores()
    {
        winCount = 0; // Reset the win count
        highScore = 0; // Reset the high score
        PlayerPrefs.SetInt("WinCount", winCount); // Update PlayerPrefs
        PlayerPrefs.SetInt("HighScore", highScore); // Update PlayerPrefs
        PlayerPrefs.Save(); // Save changes
        UpdateWinCountUI(); // Update the UI to reflect changes
        UpdateHighScoreUI(); // Update the UI to reflect changes
    }


    // Method to go back to the main menu
    private void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
