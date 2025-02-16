using UnityEngine;

public class CardSkinManager : MonoBehaviour
{
    public static CardSkinManager Instance;

    public Sprite[] skin1CardSprites;
    public Sprite[] skin2CardSprites;
    public Sprite cardBackSkin1;
    public Sprite cardBackSkin2;

    private Sprite[] currentCardSprites;
    private Sprite currentCardBack;
    private int currentSkinIndex = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Set default skin
        SetSkin(1);
    }

    public void SetSkin(int skinIndex)
    {
        currentSkinIndex = skinIndex;

        if (skinIndex == 1)
        {
            currentCardSprites = skin1CardSprites;
            currentCardBack = cardBackSkin1;
        }
        else if (skinIndex == 2)
        {
            currentCardSprites = skin2CardSprites;
            currentCardBack = cardBackSkin2;
        }

        Debug.Log("Skin set to: " + skinIndex);
    }

    public Sprite GetCardSprite(int cardID)
    {
        if (currentCardSprites != null && cardID >= 0 && cardID < currentCardSprites.Length)
        {
            return currentCardSprites[cardID];
        }
        return null;
    }

    public Sprite GetCardBackSprite()
    {
        return currentCardBack;
    }
}
