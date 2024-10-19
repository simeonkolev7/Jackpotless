using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour
{
    public Sprite[] cardSprites; // Array holding card sprites
    int[] cardValues = new int[53]; // Array to store the values of the cards
    int currentIndex = 0; // Track the current card being dealt

    void Start()
    {
        GetCardValues(); // Initialize the values of the cards
    }

    // Assign values to the cards (e.g., 2-10, J, Q, K, A)
    void GetCardValues()
    {
        int num = 0;
        // Loop through each card sprite
        for (int i = 0; i < cardSprites.Length; i++)
        {
            num = i % 13; // Reset after 13 cards (to handle 4 suits)

            // Assign card value (Aces = 1, Face cards = 10)
            if (num > 10 || num == 0)
            {
                num = 10; // Face cards and 10 have the same value
            }
            cardValues[i] = num++;
        }
    }

    // Shuffle the deck by swapping cards randomly
    public void Shuffle()
    {
        // Use Fisher-Yates shuffle algorithm
        for (int i = cardSprites.Length - 1; i > 0; --i)
        {
            int j = Mathf.FloorToInt(Random.Range(0.0f, 1.0f) * (cardSprites.Length - 1)) + 1;
            Sprite face = cardSprites[i];
            cardSprites[i] = cardSprites[j];
            cardSprites[j] = face;

            int value = cardValues[i];
            cardValues[i] = cardValues[j];
            cardValues[j] = value;
        }
        currentIndex = 1; // Reset deck pointer to start after shuffling
    }

    // Deal a card, return its value
    public int DealCard(CardScript cardScript)
    {
        // Assign the next card sprite and value to the cardScript object
        cardScript.SetSprite(cardSprites[currentIndex]);
        cardScript.SetValue(cardValues[currentIndex]);
        currentIndex++; // Move to the next card
        return cardScript.GetValueOfCard(); // Return the value of the dealt card
    }

    // Get the sprite for the back of the cards (used to hide the dealer's card)
    public Sprite GetCardBack()
    {
        return cardSprites[0]; // Assuming the first sprite is the card back
    }
}
