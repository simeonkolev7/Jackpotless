using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Game Buttons
    public Button dealBtn;
    public Button hitBtn;
    public Button standBtn;

    private int standClicks = 0;

    // Access the player and dealer's script
    public PlayerScript playerScript;
    public PlayerScript dealerScript;

    // public Text to access and update - hud
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

        // Initialize win count and high score
        UpdateWinCountUI();
        UpdateHighScoreUI();
    }

    private void DealClicked()
    {
        // Reset round, hide text, prep for new hand
        playerScript.ResetHand();
        dealerScript.ResetHand();
        // Hide deal hand score at start of deal
        dealerScoreText.gameObject.SetActive(false);
        mainText.gameObject.SetActive(false);
        dealerScoreText.gameObject.SetActive(false);
        GameObject.Find("Deck").GetComponent<DeckScript>().Shuffle();
        playerScript.StartHand();
        dealerScript.StartHand();
        // Update the scores displayed
        scoreText.text = "Hand: " + playerScript.handValue.ToString();
        dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
        // Place card back on dealer card, hide card
        hideCard.GetComponent<Renderer>().enabled = true;
        // Adjust buttons visibility
        dealBtn.gameObject.SetActive(false);
        hitBtn.gameObject.SetActive(true);
        standBtn.gameObject.SetActive(true);
        standBtnText.text = "Stand";
    }

    private void HitClicked()
    {
        // Check that there is still room on the table
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

    // Check for winnner and loser, hand is over
    void RoundOver()
    {
        // Booleans (true/false) for bust and blackjack/21
        bool playerBust = playerScript.handValue > 21;
        bool dealerBust = dealerScript.handValue > 21;
        bool player21 = playerScript.handValue == 21;
        bool dealer21 = dealerScript.handValue == 21;
        // If stand has been clicked less than twice, no 21s or busts, quit function
        if (standClicks < 2 && !playerBust && !dealerBust && !player21 && !dealer21) return;
        bool roundOver = true;
        // All bust, round over
        if (playerBust && dealerBust)
        {
            mainText.text = "All Bust: No winners";
        }
        // if player busts, dealer wins
        else if (playerBust || (!dealerBust && dealerScript.handValue > playerScript.handValue))
        {
            mainText.text = "Dealer wins!";
        }
        // if dealer busts or player wins
        else if (dealerBust || playerScript.handValue > dealerScript.handValue)
        {
            mainText.text = "You win!";
            // Increase win count and high score
            winCount++;
            highScore += 10; // Example: increment score by 10 for each win
            UpdateWinCountUI();
            UpdateHighScoreUI();
        }
        //Check for tie
        else if (playerScript.handValue == dealerScript.handValue)
        {
            mainText.text = "Push: It's a tie!";
        }
        else
        {
            roundOver = false;
        }
        // Set UI up for next move / hand / turn
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

    // Update the win count text
    private void UpdateWinCountUI()
    {
        winCountText.text = "Wins: " + winCount.ToString();
    }

    // Update the high score text
    private void UpdateHighScoreUI()
    {
        highScoreText.text = "High Score: " + highScore.ToString();
    }
}


