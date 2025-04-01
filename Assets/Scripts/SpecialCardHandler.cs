using UnityEngine;
using UnityEngine.UI;

public enum SpecialCardType { None, Limit25, Limit17 }

public class SpecialCardHandler : MonoBehaviour
{
    private SpecialCardType currentCard = SpecialCardType.None;

    [Header("UI")]
    public Text specialCardText; // Drag your UI Text here in the Inspector

    private void Start()
    {
        // If you want to start with a card assigned automatically:
        // AssignNewCard();
    }

    public void AssignNewCard()
    {
        int roll = Random.Range(0, 3); // 0 = None, 1 = Limit25, 2 = Limit17
        currentCard = (SpecialCardType)roll;

        Debug.Log($"[SpecialCardHandler] Assigned: {currentCard}");
        UpdateText(); // Ensure UI always updates after assignment
    }

    public int GetCurrentLimit()
    {
        return currentCard switch
        {
            SpecialCardType.Limit25 => 25,
            SpecialCardType.Limit17 => 17,
            _ => 21,
        };
    }

    public SpecialCardType GetCurrentCardType()
    {
        return currentCard;
    }

    private void UpdateText()
    {
        if (specialCardText == null)
        {
            Debug.LogWarning("[SpecialCardHandler] No Text reference set!");
            return;
        }

        string display = currentCard switch
        {
            SpecialCardType.Limit25 => "Special Rule: Limit 25!",
            SpecialCardType.Limit17 => "Special Rule: Limit 17!",
            _ => "Standard Blackjack Rules"
        };

        specialCardText.text = display;
        Debug.Log("[SpecialCardHandler] UI Updated: " + display);
    }
}

